namespace Modisette.Models;

public class EmailMessage
{
    public List<EmailAddress> FromEmailAddress { get; set; } = new();
    public List<EmailAddress> ToEmailAddress { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}