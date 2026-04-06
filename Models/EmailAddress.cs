using System.ComponentModel.DataAnnotations;

namespace Modisette.Models;

public class EmailAddress
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Address { get; set; } = string.Empty;
}
