using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modisette.Models;
public class Contact
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Message { get; set; }
    [Display(Name = "Time Submitted")]
    public DateTime TimeSubmitted { get; set; }

    public string? Notes { get; set; } = "";

    public Contact()
    {
        TimeSubmitted = DateTime.UtcNow;
    }

}