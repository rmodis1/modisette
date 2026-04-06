using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Modisette.Models;

namespace Modisette.Services;

public sealed class AdminAuthenticationService : IAdminAuthenticationService
{
    private const string Pbkdf2Prefix = "PBKDF2";
    private readonly AdminAuthOptions _options;

    public AdminAuthenticationService(IOptions<AdminAuthOptions> options)
    {
        _options = options.Value;
    }

    public AdminAuthenticationResult Authenticate(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(_options.Username) ||
            (string.IsNullOrWhiteSpace(_options.Password) && string.IsNullOrWhiteSpace(_options.PasswordHash)))
        {
            return new AdminAuthenticationResult(false, "Admin login is not configured yet.");
        }

        if (!string.Equals(username.Trim(), _options.Username.Trim(), StringComparison.Ordinal))
        {
            return new AdminAuthenticationResult(false, "Invalid username or password.");
        }

        var passwordIsValid = !string.IsNullOrWhiteSpace(_options.PasswordHash)
            ? VerifyPasswordHash(password, _options.PasswordHash)
            : VerifyPlaintextPassword(password, _options.Password);

        return passwordIsValid
            ? new AdminAuthenticationResult(true)
            : new AdminAuthenticationResult(false, "Invalid username or password.");
    }

    private static bool VerifyPlaintextPassword(string password, string configuredPassword)
    {
        var left = Encoding.UTF8.GetBytes(password);
        var right = Encoding.UTF8.GetBytes(configuredPassword);
        return CryptographicOperations.FixedTimeEquals(left, right);
    }

    private static bool VerifyPasswordHash(string password, string configuredHash)
    {
        var parts = configuredHash.Split('$', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4 || !string.Equals(parts[0], Pbkdf2Prefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var iterations) || iterations <= 0)
        {
            return false;
        }

        byte[] salt;
        byte[] expectedHash;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expectedHash = Convert.FromBase64String(parts[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}