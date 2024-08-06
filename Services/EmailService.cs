using MailKit.Security;
using Microsoft.VisualBasic;
using MimeKit;
using Modisette.Models;

namespace Modisette.Services;

public class MailKitEmailService: IEmailService
{
    private readonly EmailServerConfiguration _eConfig;
    public MailKitEmailService(EmailServerConfiguration config)
    {
        _eConfig = config;
    }

    public async Task Send(EmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.From.AddRange(emailMessage.FromEmailAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
        message.To.AddRange(emailMessage.ToEmailAddress.Select(x => new MailboxAddress(x.Name, x.Address)));
        message.Subject = emailMessage.Subject;
        message.Body = new TextPart("plain")
        {
            Text = emailMessage.Content
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync(_eConfig.SmtpServer, _eConfig.SmtpPort, true);

            await client.AuthenticateAsync(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}