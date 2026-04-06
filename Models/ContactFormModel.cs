using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modisette.Models;
public class Contact
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Phone]
    [StringLength(25)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
    [Display(Name = "Time Submitted")]
    public DateTime TimeSubmitted { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; } = "";

    public Contact()
    {
        TimeSubmitted = DateTime.UtcNow;
    }

}