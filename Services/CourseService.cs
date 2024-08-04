using System.Runtime.CompilerServices;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;
using Modisette.Repositories;

namespace Modisette.Services;

public class CourseService : ICourseService
{
    private readonly SiteContext _context;

    public CourseService(SiteContext context)
    {
        _context = context;
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

    public async Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode)
    {
        return await _context.CourseDocuments
            .Where(file => file.CourseCode == courseCode)
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

    public async Task DeleteCourseAsync(Course course)
    {
        var courseToDelete = await GetCourseAsync(course);
        if (courseToDelete != null)
        {
            _context.Courses.Remove(courseToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Attach(course).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task SaveCourseAsync(Course course)
    {
        _context.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CourseExistsAsync(string courseCode)
    {
        return await _context.CourseExistsAsync(courseCode);
    }
}