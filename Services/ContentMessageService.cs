using Modisette.Models;

public class ContactMessageBuilder : IContactMessageBuilder
{
    private readonly EmailAddress _fromAndToEmailAddress;

    public ContactMessageBuilder(EmailAddress fromAndToEmailAddress)
    {
        _fromAndToEmailAddress = fromAndToEmailAddress;
    }

    public EmailMessage BuildMessage(Contact contact)
    {
        return new EmailMessage
        {
            FromEmailAddress = new List<EmailAddress> { _fromAndToEmailAddress },
            ToEmailAddress = new List<EmailAddress> { _fromAndToEmailAddress },
            Content = $"Someone just contacted you through your website!\n" +
                      $"Name: {contact.FirstName} {contact.LastName}\n" +
                      $"Email: {contact.Email}\n" +
                      $"Message: {contact.Message}",
            Subject = "Contact Form"
        };
    }
}

public interface IContactMessageBuilder
{
    EmailMessage BuildMessage(Contact contact);
}