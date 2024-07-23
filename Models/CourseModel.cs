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

[PrimaryKey(nameof(Number), nameof(Year), nameof(Semester))]
public class Course
{
    [Required]
    public int Number { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public TimeOfYear Semester { get; set; }
    [Required]
    public string Title { get; set; }
    public ICollection<CourseDocument> Files { get; set; } = new List<CourseDocument>();

}