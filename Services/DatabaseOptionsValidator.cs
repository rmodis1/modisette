using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

public sealed class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionsValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ValidateOptionsResult Validate(string? name, DatabaseOptions options)
    {
        var provider = SiteContextConfiguration.NormalizeProvider(options.Provider);

        if (provider is not (SiteContextConfiguration.SqliteProvider or SiteContextConfiguration.PostgresProvider))
        {
            return ValidateOptionsResult.Fail("Database:Provider must be either 'sqlite' or 'postgres'.");
        }

        var connectionStringName = provider == SiteContextConfiguration.PostgresProvider ? "Postgres" : "SiteContext";
        if (string.IsNullOrWhiteSpace(_configuration.GetConnectionString(connectionStringName)))
        {
            return ValidateOptionsResult.Fail($"ConnectionStrings:{connectionStringName} is required when Database:Provider is '{provider}'.");
        }

        return ValidateOptionsResult.Success;
    }
}