using Modisette.Models;

namespace Modisette.Services
{
    //Single Responsibility Principle (SRP): This class is responsible for building a message to be sent to the website owner when a user submits the contact form.
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
}