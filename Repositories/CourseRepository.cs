using Modisette.Models;
using Modisette.Data;
using Microsoft.EntityFrameworkCore;

namespace Modisette.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly SiteContext _context;

    public CourseRepository(SiteContext context)
    {
        _context = context;
    }

    public object Courses { get; internal set; }

    public async Task AddCourseAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task AddCourseDocumentAsync(CourseDocument courseDocument)
    {
        _context.CourseDocuments.Add(courseDocument);
        await _context.SaveChangesAsync();
    }

    public async Task<Course?> GetCourseByCodeAsync(string code)
    {
        return await _context.Courses.FirstOrDefaultAsync(course => course.Code == code);
    }
}