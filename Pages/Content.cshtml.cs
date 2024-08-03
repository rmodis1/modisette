using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Services;

namespace Modisette.Pages;

public class ContentModel : PageModel
{
    // Dependency Inversion Principle (DIP): Depend on abstractions (ICourseService) rather than concrete implementations.
    private readonly ICourseService _courseService;

     // Constructor Injection: Follows the Dependency Injection principle, which is part of DIP.
    public ContentModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [BindProperty(SupportsGet = true)]
    public int? Year { get; set; }

    [BindProperty(SupportsGet = true)]
    public TimeOfYear? Semester { get; set; }

    [BindProperty(SupportsGet = true)]
    public string CourseCode { get; set; }

    public List<SelectListItem> Years { get; set; }
    public List<SelectListItem> Semesters { get; set; }
    public List<SelectListItem> CourseCodes { get; set; }
    public List<CourseDocument> CourseDocuments { get; set; }

    // Single Responsibility Principle (SRP): The OnGetAsync method is responsible for handling the GET request and populating the properties.
    // Open/Closed Principle (OCP): The method is open for extension (via ICourseService) but closed for modification.
    public async Task OnGetAsync()
    {
        //By using a service to retrieve data, we abstract unnecessary responsibility from the page model, following the SRP.
        Years = await _courseService.GetYearsAsync();

        Semesters = new List<SelectListItem>();
        CourseCodes = new List<SelectListItem>();
        CourseDocuments = new List<CourseDocument>();

        if (Year.HasValue)
        {
            Semesters = await _courseService.GetSemestersAsync(Year.Value);
        }

        if (Year.HasValue && Semester.HasValue)
        {
            CourseCodes = await _courseService.GetCourseCodesAsync(Year.Value, Semester.Value);
        }

        if (!string.IsNullOrEmpty(CourseCode))
        {
           CourseDocuments = await _courseService.GetCourseDocumentsAsync(CourseCode);
        }
    }
}

