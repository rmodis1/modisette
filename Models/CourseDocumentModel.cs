using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Modisette.Models;

public class CourseDocument
{
    [Key]
    public int Id { get; set; }
    public required string CourseCode { get; set; }
    public int CourseYear { get; set; }
    public TimeOfYear CourseSemester { get; set; }
    public required string Name { get; set; }
    public required Uri Document { get; set; }

}