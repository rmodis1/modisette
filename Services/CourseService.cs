using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

public class CourseService : ICourseService
{
    private readonly SiteContext _context;

    public CourseService(SiteContext context)
    {
        _context = context;
    }

    public async Task AddCourseAsync(Course course)
    {
          if (course == null)
        {
        throw new ArgumentNullException(nameof(course));
        }

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SelectListItem>> GetYearsAsync()
    {
        return await _context.Courses.Select(c => new SelectListItem
        {
            Value = c.Year.ToString(),
            Text = c.Year.ToString()
        }).Distinct()
          .ToListAsync();
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
            .Where(course => course.Year == year && course.Semester == semester)
            .Select(course => course.Code)
            .Distinct()
            .Select(courses => new SelectListItem { Value = courses, Text = courses })
            .ToListAsync();
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

    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<bool> UpdateCourseAsync(Course course)
    {
        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        _context.Attach(course).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency issues
            // Log the exception details
            Console.WriteLine($"Concurrency exception: {ex.Message}");
            return false;
        }
        catch (DbUpdateException ex)
        {
            // Handle other database update issues
            // Log the exception details
            Console.WriteLine($"Database update exception: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            // Log the exception details
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return false;
        }
    }

      public async Task DeleteCourseAsync(Course course)
    {
        var courseToDelete = await GetCourseAsync(course);
        if (courseToDelete != null)
        {
            _context.Courses.Remove(courseToDelete);
            await _context.SaveChangesAsync();
        }
    }
}