using Microsoft.EntityFrameworkCore;

namespace Modisette.Data;

public static class SiteContextConfiguration
{
    public const string SqliteProvider = "sqlite";
    public const string PostgresProvider = "postgres";
    public const string PostgresMigrationAssembly = "Modisette.PostgresMigration";

    public static void Configure(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var provider = NormalizeProvider(configuration[$"{Models.DatabaseOptions.SectionName}:Provider"]);

        if (provider == PostgresProvider)
        {
            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(PostgresMigrationAssembly));
            return;
        }

        if (provider == SqliteProvider)
        {
            var connectionString = configuration.GetConnectionString("SiteContext")
                ?? throw new InvalidOperationException("Connection string 'SiteContext' not found.");

            options.UseSqlite(connectionString);
            return;
        }

        throw new InvalidOperationException("Database:Provider must be either 'sqlite' or 'postgres'.");
    }

    public static string NormalizeProvider(string? provider)
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            return SqliteProvider;
        }

        return provider.Trim().ToLowerInvariant() switch
        {
            "postgresql" => PostgresProvider,
            var value => value
        };
    }
}