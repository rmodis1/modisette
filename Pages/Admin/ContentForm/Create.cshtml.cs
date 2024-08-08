using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modisette.Data;
using Modisette.Models;
using Modisette.Services;

namespace modisette.Pages.Admin.ContentForm
{
    public class CreateModel : PageModel
    {
        // Dependency Inversion Principle (DIP): the CreateModel class depends on abstractions (ICourseService and IFileService) rather than concrete implementations.
        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

        // Constructor Injection: this follows the Dependency Inversion Principle (DIP) by injecting dependencies through the constructor.
        public CreateModel (ICourseService courseService, IFileService fileService)
        {
            _courseService = courseService;
            _fileService = fileService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;
        [BindProperty]
        public BufferedFiles Files { get; set; }

        // Consider if overposting will be an issue.
        // Single Responsibility Principle (SRP): the OnPostAsync method is responsible only for handling POST requests, validating the model, and invoking services.
        // Open/Closed Principle (OCP): the method is open for extension (by adding more services or logic) but closed for modification (the core logic remains unchanged). 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _courseService.AddCourseAsync(Course);
            await _fileService.UploadFilesAsync(Files, Course);
            return RedirectToPage("./Index");
        }
    }
}
