using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modisette.Data;
using Modisette.Models;
using Modisette.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services.AddOptions<AdminAuthOptions>()
                .Bind(builder.Configuration.GetSection(AdminAuthOptions.SectionName))
                .ValidateOnStart();
builder.Services.AddOptions<EmailServerConfiguration>()
                .Bind(builder.Configuration.GetSection("EmailConfiguration"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
builder.Services.AddOptions<EmailAddress>()
                .Bind(builder.Configuration.GetSection("SiteEmailAddress"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

builder.Services.AddSingleton<Microsoft.Extensions.Options.IValidateOptions<AdminAuthOptions>, AdminAuthOptionsValidator>();

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

builder.Services.AddDbContext<SiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SiteContext") 
    ?? throw new InvalidOperationException("Connection string 'SiteContext' not found.")));

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSingleton<IAdminAuthenticationService, AdminAuthenticationService>();
builder.Services.AddTransient<ITwitterTimelineService, TwitterTimelineService>();
builder.Services.AddScoped<IContactMessageBuilder, ContactMessageBuilder>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailServerConfiguration>>().Value);
builder.Services.AddTransient<IEmailService, MailKitEmailService>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailAddress>>().Value); 

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
