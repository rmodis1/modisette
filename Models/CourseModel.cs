using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Modisette.Models;

public enum TimeOfYear
{
    Spring,
    Summer,
    Fall,
    Winter
}

[PrimaryKey(nameof(Code), nameof(Year), nameof(Semester))]
public class Course
{
    [Required]
    [StringLength(32)]
    public string Code { get; set; } = string.Empty;
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }
    [Required]
    public TimeOfYear Semester { get; set; }
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public ICollection<CourseDocument> Files { get; set; } = new List<CourseDocument>();

}