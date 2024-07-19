namespace Modisette.Models;

public class EmailMessage
{
    public List<EmailAddress> FromEmailAddress { get; set; }
    public List<EmailAddress> ToEmailAddress { get; set;}
    public string Subject { get; set; }
    public string Content { get; set; }
}