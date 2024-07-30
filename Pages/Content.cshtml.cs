using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;

namespace Modisette.Pages;

public class ContentModel : PageModel
{
    private readonly Modisette.Data.SiteContext _context;

    public ContentModel(Modisette.Data.SiteContext context)
    {
        _context = context;
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

    public async Task OnGetAsync()
    {
        Years = await _context.Courses.Select(c => new SelectListItem
        {
            Value = c.Year.ToString(),
            Text = c.Year.ToString()
        }).Distinct().ToListAsync();

        Semesters = new List<SelectListItem>();
        CourseCodes = new List<SelectListItem>();
        CourseDocuments = new List<CourseDocument>();

        if (Year.HasValue)
        {
            Semesters = await _context.Courses
                .Where(c => c.Year == Year.Value)
                .Select(c => c.Semester)
                .Distinct()
                .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() })
                .ToListAsync();
        }

        if (Year.HasValue && Semester.HasValue)
        {
            CourseCodes = await _context.Courses
                .Where(c => c.Year == Year.Value && c.Semester == Semester.Value)
                .Select(c => c.Code)
                .Distinct()
                .Select(cc => new SelectListItem { Value = cc, Text = cc })
                .ToListAsync();
        }

        if (!string.IsNullOrEmpty(CourseCode))
        {
            CourseDocuments = await _context.CourseDocuments
                .Where(cd => cd.CourseCode == CourseCode)
                .ToListAsync();
        }
    }
}

