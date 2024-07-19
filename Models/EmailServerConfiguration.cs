namespace Modisette.Models;

public class EmailServerConfiguration
{
    public string From { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpServer { get; set;}
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
}