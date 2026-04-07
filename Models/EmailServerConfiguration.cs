using System.ComponentModel.DataAnnotations;

namespace Modisette.Models;

public class EmailServerConfiguration
{
    [Required]
    [EmailAddress]
    public string From { get; set; } = string.Empty;

    [Required]
    public string ResendApiKey { get; set; } = string.Empty;
}