using Modisette.Models;
using Resend;
using AppEmailAddress = Modisette.Models.EmailAddress;
using AppEmailMessage = Modisette.Models.EmailMessage;

namespace Modisette.Services;

//Single Responsibility Principle (SRP): This class is responsible only for sending an email to the client.
public class ResendEmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly EmailServerConfiguration _eConfig;
    private readonly ILogger<ResendEmailService> _logger;

    public ResendEmailService(IResend resend, EmailServerConfiguration config, ILogger<ResendEmailService> logger)
    {
        _resend = resend;
        _eConfig = config;
        _logger = logger;
    }

    public async Task Send(AppEmailMessage message)
    {
        var resendMessage = new Resend.EmailMessage
        {
            From = _eConfig.From,
            Subject = message.Subject,
            TextBody = message.Content
        };

        foreach (var recipient in message.ToEmailAddress)
        {
            resendMessage.To.Add(FormatAddress(recipient));
        }

        var response = await _resend.EmailSendAsync(resendMessage);
        _logger.LogInformation("Sent email notification through Resend with id {EmailId}.", response.Content);
    }

    private static string FormatAddress(AppEmailAddress address)
    {
        return string.IsNullOrWhiteSpace(address.Name)
            ? address.Address
            : $"{address.Name} <{address.Address}>";
    }
}