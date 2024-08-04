using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

public class CourseService : ICourseService
{
    private readonly SiteContext _context;

    public CourseService(SiteContext context, IWebHostEnvironment webHostEnvironment)
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

     public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetCourseByCodeAsync(string code)
    {
        return await _context.Courses.FirstOrDefaultAsync(course => course.Code == code);
    }

    public async Task<Course?> GetCourseAsync(Course course)
    {
        return await _context.Courses.SingleOrDefaultAsync(m => m.Code == course.Code &&
                                                                m.Year == course.Year &&
                                                            m.Semester == course.Semester);
    }

    public async Task AddCourseAsync(Course course)    
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateCourseAsync(Course course)
    {
        _context.Attach(course).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(await CourseExistsAsync(course.Code)))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> CourseExistsAsync(string code)
    {
        return await _context.Courses.AnyAsync(e => e.Code == code);
    }

    public async Task DeleteCourseAsync(Course courseToDelete)
    {
        _context.Courses.Remove(courseToDelete);
        await _context.SaveChangesAsync();
    }
}