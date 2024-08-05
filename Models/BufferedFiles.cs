using System.ComponentModel.DataAnnotations;

namespace Modisette.Models;

public class BufferedFiles
{
    [Required]
    [Display(Name = "File")]
    public List<IFormFile>? FormFiles { get; set; }
}