namespace Modisette.Models;

public interface IEmailService
{
    Task Send(EmailMessage message);
}