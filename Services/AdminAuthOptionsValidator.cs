using Microsoft.Extensions.Options;
using Modisette.Models;

namespace Modisette.Services;

public sealed class AdminAuthOptionsValidator : IValidateOptions<AdminAuthOptions>
{
    public ValidateOptionsResult Validate(string? name, AdminAuthOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Username))
        {
            return ValidateOptionsResult.Fail("AdminAuth:Username is required.");
        }

        var hasPlaintextPassword = !string.IsNullOrWhiteSpace(options.Password);
        var hasPasswordHash = !string.IsNullOrWhiteSpace(options.PasswordHash);

        if (!hasPlaintextPassword && !hasPasswordHash)
        {
            return ValidateOptionsResult.Fail("Set either AdminAuth:Password or AdminAuth:PasswordHash.");
        }

        return ValidateOptionsResult.Success;
    }
}