using MailKit.Security;
using Microsoft.VisualBasic;
using MimeKit;
using Modisette.Models;

namespace Modisette.Services;

//Single Responsibility Principle (SRP): This class is responsible only for sending an email to the client.
public class MailKitEmailService: IEmailService
{
    private readonly EmailServerConfiguration _eConfig;

    public MailKitEmailService(EmailServerConfiguration config)
    {
        _eConfig = config;
    }

    public async Task Send(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.AddRange(message.FromEmailAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
        mimeMessage.To.AddRange(message.ToEmailAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart("plain")
        {
            Text = message.Content
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

            await client.ConnectAsync(_eConfig.SmtpServer, _eConfig.SmtpPort, GetSecureSocketOptions());

            await client.AuthenticateAsync(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }

    private SecureSocketOptions GetSecureSocketOptions()
    {
        return Enum.TryParse<SecureSocketOptions>(_eConfig.SecureSocketOptions, ignoreCase: true, out var secureSocketOptions)
            ? secureSocketOptions
            : SecureSocketOptions.Auto;
    }
}