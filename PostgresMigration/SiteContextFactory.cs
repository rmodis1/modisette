using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.PostgresMigration;

public sealed class SiteContextFactory : IDesignTimeDbContextFactory<SiteContext>
{
    public SiteContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var basePath = ResolveBasePath();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<AdminAuthOptions>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SiteContext>();
        SiteContextConfiguration.Configure(
            optionsBuilder,
            configuration,
            SiteContextConfiguration.PostgresMigrationAssembly);

        return new SiteContext(optionsBuilder.Options);
    }

    private static string ResolveBasePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var appSettingsPath = Path.Combine(currentDirectory, "appsettings.json");
        if (File.Exists(appSettingsPath))
        {
            return currentDirectory;
        }

        var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        if (parentDirectory is not null && File.Exists(Path.Combine(parentDirectory, "appsettings.json")))
        {
            return parentDirectory;
        }

        throw new InvalidOperationException("Could not locate appsettings.json for design-time SiteContext creation.");
    }
}