using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modisette.Data;
using Modisette.Models;

namespace modisette.Pages.Admin.ContentForm
{
    public class CreateModel : PageModel
    {

         private readonly Modisette.Data.SiteContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel (Modisette.Data.SiteContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;
        [BindProperty]
        public BufferedFiles Files { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();
            await OnPostUploadAsync(Course);

            return RedirectToPage("./Index");
        }

        public async Task OnPostUploadAsync(Course course)
        {
            foreach (var formFile in Files.FormFiles)
            {
                if (formFile.Length > 0)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var courseDocument = new CourseDocument
                    {
                        CourseCode = course.Code,
                        CourseYear = course.Year,
                        CourseSemester = course.Semester,
                        Name = fileName,
                        Document = new Uri(fileName, UriKind.Relative)
                    };

                    _context.CourseDocuments.Add(courseDocument);
                }
            }

            await _context.SaveChangesAsync();

        }
    }

    public class BufferedFiles
    {
        [Required]
        public List<IFormFile> FormFiles { get; set; }
    }

    public static class FileHelpers
    {
        public static async Task<byte[]> ProcessFormFile<T>(IFormFile formFile, ModelStateDictionary modelState)
        {
            var fieldDisplayName = string.Empty;

            MemberInfo property = 
                typeof(T).GetProperty(formFile.Name.Substring(
                    formFile.Name.IndexOf(".", StringComparison.Ordinal) + 1));

            if (property != null)
            {
                if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                        DisplayAttribute displayAttribute)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                    formFile.FileName);

            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, 
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");

                return Array.Empty<byte>();
            }
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                    $"Please contact the Help Desk for support. Error: {ex.HResult}");
                // Log the exception
            }

            return Array.Empty<byte>();
        }
    }

    //     private readonly Modisette.Data.SiteContext _context;

    //     public CreateModel(Modisette.Data.SiteContext context)
    //     {
    //         _context = context;
    //     }

    //     public IActionResult OnGet()
    //     {
    //         return Page();
    //     }

    //     [BindProperty]
    //     public Course Course { get; set; } = default!;

    //     // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    //     public async Task<IActionResult> OnPostAsync()
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return Page();
    //         }

    //         _context.Courses.Add(Course);
    //         await _context.SaveChangesAsync();

    //         return RedirectToPage("./Index");
    //     }
    // }
}
