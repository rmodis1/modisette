namespace Modisette.Services;

public interface IAdminAuthenticationService
{
    AdminAuthenticationResult Authenticate(string username, string password);
}

public sealed record AdminAuthenticationResult(bool Succeeded, string? ErrorMessage = null);