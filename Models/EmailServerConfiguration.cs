using System.ComponentModel.DataAnnotations;

namespace Modisette.Models;

public class EmailServerConfiguration
{
    [Required]
    [EmailAddress]
    public string From { get; set; } = string.Empty;

    [Range(1, 65535)]
    public int SmtpPort { get; set; } = 465;

    [Required]
    public string SmtpServer { get; set; } = string.Empty;

    [Required]
    public string SmtpUsername { get; set; } = string.Empty;

    [Required]
    public string SmtpPassword { get; set; } = string.Empty;
}