using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Modisette.Data;

public sealed class SiteContextFactory : IDesignTimeDbContextFactory<SiteContext>
{
    public SiteContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<SiteContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SiteContext>();
        SiteContextConfiguration.Configure(optionsBuilder, configuration);
        return new SiteContext(optionsBuilder.Options);
    }
}