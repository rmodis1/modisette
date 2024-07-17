using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modisette.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services
       .AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = builder.Configuration["Auth0:Domain"];
            options.ClientId = builder.Configuration["Auth0:ClientId"];
        });

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/ContactForm")
                       .AllowAnonymousToPage("/ContactForm/Display")
                       .AllowAnonymousToFolder("/ContactForm/Account");
});

builder.Services.AddDbContext<ContactFormContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContactFormContext") 
    ?? throw new InvalidOperationException("Connection string 'ContactFormContext' not found.")));

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
