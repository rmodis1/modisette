using Modisette.Models;

namespace Modisette.Services;

public interface IEmailService
{
    Task Send(EmailMessage message);
}