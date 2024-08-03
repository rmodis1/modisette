using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;

namespace Modisette.Services;
public interface ICourseService
{
    Task<List<SelectListItem>> GetYearsAsync();
    Task<List<SelectListItem>> GetSemestersAsync(int year);
    Task<List<SelectListItem>> GetCourseCodesAsync(int year, TimeOfYear semester);
    Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode);
}

public class CourseService : ICourseService
{
    private readonly Modisette.Data.SiteContext _context;

    public CourseService(Modisette.Data.SiteContext context)
    {
        _context = context;
    }

    public async Task<List<SelectListItem>> GetYearsAsync()
    {
        return await _context.Courses.Select(c => new SelectListItem
        {
            Value = c.Year.ToString(),
            Text = c.Year.ToString()
        }).Distinct().ToListAsync();
    }

    public async Task<List<SelectListItem>> GetSemestersAsync(int year)
    {
        return await _context.Courses
            .Where(c => c.Year == year)
            .Select(c => c.Semester)
            .Distinct()
            .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() })
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetCourseCodesAsync(int year, TimeOfYear semester)
    {
        return await _context.Courses
            .Where(c => c.Year == year && c.Semester == semester)
            .Select(c => c.Code)
            .Distinct()
            .Select(cc => new SelectListItem { Value = cc, Text = cc })
            .ToListAsync();
    }

    public async Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode)
    {
        return await _context.CourseDocuments
            .Where(cd => cd.CourseCode == courseCode)
            .ToListAsync();
    }
}