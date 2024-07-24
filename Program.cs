using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modisette.Data;
using Modisette.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services
       .AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = builder.Configuration["Auth0:Domain"];
            options.ClientId = builder.Configuration["Auth0:ClientId"];
        });

builder.Services.AddMvc();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/ContactForm")
                       .AllowAnonymousToPage("/ContactForm/Index")
                       .AllowAnonymousToFolder("/ContactForm/Account");
});

builder.Services.AddDbContext<SiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SiteContext") 
    ?? throw new InvalidOperationException("Connection string 'SiteContext' not found.")));

var emailConfig = builder.Configuration
                         .GetSection("EmailConfiguration")
                         .Get<EmailServerConfiguration>();

emailConfig.SmtpPassword = builder.Configuration["SmtpPassword"];

builder.Services.AddSingleton<EmailServerConfiguration>(emailConfig);

builder.Services.AddTransient<IEmailService, MailKitEmailService>();

var emailAddress = builder.Configuration
                          .GetSection("SiteEmailAddress")
                          .Get<EmailAddress>();

builder.Services.AddSingleton<EmailAddress>(emailAddress); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
