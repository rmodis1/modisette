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

        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
