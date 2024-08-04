using Modisette.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface ICourseService
{
    Task AddCourseAsync(Course course);
    Task AddCourseDocumentAsync(CourseDocument courseDocument);
    Task<bool> CourseExistsAsync(string courseCode);
    Task<List<SelectListItem>> GetYearsAsync();
    Task<List<SelectListItem>> GetSemestersAsync(int year);
    Task<List<SelectListItem>> GetCourseCodesAsync(int year, TimeOfYear semester);
    Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode);
    Task<Course?> GetCourseByCodeAsync(string code);
    Task<Course?> GetCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(Course course);
}