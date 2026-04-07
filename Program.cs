using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modisette.Data;
using Modisette.Models;
using Modisette.Services;
using Resend;
using AppEmailAddress = Modisette.Models.EmailAddress;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (!string.IsNullOrWhiteSpace(port) && string.IsNullOrWhiteSpace(urls))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

var dataProtectionBuilder = builder.Services.AddDataProtection()
                                          .SetApplicationName("modisette");
var dataProtectionKeysDirectory = builder.Configuration["DataProtection:KeysDirectory"];
if (!string.IsNullOrWhiteSpace(dataProtectionKeysDirectory))
{
    Directory.CreateDirectory(dataProtectionKeysDirectory);
    dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysDirectory));
}

// Add services.
builder.Services.AddOptions<AdminAuthOptions>()
                .Bind(builder.Configuration.GetSection(AdminAuthOptions.SectionName))
                .ValidateOnStart();
builder.Services.AddOptions<DatabaseOptions>()
                .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName))
                .ValidateOnStart();
builder.Services.AddOptions<EmailServerConfiguration>()
                .Bind(builder.Configuration.GetSection("EmailConfiguration"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
builder.Services.AddOptions<AppEmailAddress>()
                .Bind(builder.Configuration.GetSection("SiteEmailAddress"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
builder.Services.AddOptions<ResendClientOptions>()
                .Configure<IOptions<EmailServerConfiguration>>((options, emailConfig) =>
                {
                    options.ApiToken = emailConfig.Value.ResendApiKey;
                });

builder.Services.AddSingleton<Microsoft.Extensions.Options.IValidateOptions<AdminAuthOptions>, AdminAuthOptionsValidator>();
builder.Services.AddSingleton<Microsoft.Extensions.Options.IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Admin/Account/Login";
                    options.AccessDeniedPath = "/Admin/Account/Login";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
});

builder.Services.AddHttpClient();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly")
                       .AllowAnonymousToPage("/Admin/Index")
                       .AllowAnonymousToPage("/Admin/Account/Login");
});

builder.Services.AddDbContext<SiteContext>((sp, options) =>
    SiteContextConfiguration.Configure(options, sp.GetRequiredService<IConfiguration>()));

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSingleton<IAdminAuthenticationService, AdminAuthenticationService>();
builder.Services.AddTransient<ITwitterTimelineService, TwitterTimelineService>();
builder.Services.AddScoped<IContactMessageBuilder, ContactMessageBuilder>();
builder.Services.AddSingleton<IBackgroundEmailQueue, BackgroundEmailQueue>();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailServerConfiguration>>().Value);
builder.Services.AddTransient<IEmailService, ResendEmailService>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppEmailAddress>>().Value);
builder.Services.AddHostedService<BackgroundEmailSenderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    context.Response.Headers["Content-Security-Policy"] = "base-uri 'self'; form-action 'self'; frame-ancestors 'self'; object-src 'none'";

    await next();
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
