using Modisette.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using modisette.Pages.Admin.ContentForm;

namespace Modisette.Services;

public interface ICourseService
{
    Task AddCourseAsync(Course course);
    Task<List<SelectListItem>> GetYearsAsync();
    Task<List<SelectListItem>> GetSemestersAsync(int year);
    Task<List<SelectListItem>> GetCourseCodesAsync(int year, TimeOfYear semester);
    Task<Course?> GetCourseByCodeAsync(string code);
    Task<Course?> GetCourseAsync(Course course);
    Task<List<Course>> GetCoursesAsync();
    Task<bool> UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(Course course);
}