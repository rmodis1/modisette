using Modisette.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using modisette.Pages.Admin.ContentForm;

public interface ICourseService
{
    Task AddCourseAsync(Course course);
    Task<bool> CourseExistsAsync(string courseCode);
    Task<List<Course>> GetCoursesAsync();
    Task<List<SelectListItem>> GetYearsAsync();
    Task<List<SelectListItem>> GetSemestersAsync(int year);
    Task<List<SelectListItem>> GetCourseCodesAsync(int year, TimeOfYear semester);
    Task<Course?> GetCourseByCodeAsync(string code);
    Task<Course?> GetCourseAsync(Course course);
    Task<bool> UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(Course course);
}