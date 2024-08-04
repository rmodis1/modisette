using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;
using Modisette.Services;

namespace modisette.Pages.Admin.ContentForm
{
    public class EditModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

        public EditModel(ICourseService courseService, IFileService fileService)
        {
            _courseService = courseService;
            _fileService = fileService;
        }
        [BindProperty]
        public IList<CourseDocument> Documents { get;set; } = default!;

        [BindProperty]
        public Course Course { get; set; } = default!;
        [BindProperty]
        public BufferedFiles Files { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course =  await _courseService.GetCourseByCodeAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            Course = course;

            Documents = await _fileService.GetCourseDocumentsAsync(course);
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updateResult = await _courseService.UpdateCourseAsync(Course);

            if (!updateResult)
            {
                return NotFound();
            }

            await _fileService.UploadFilesAsync(Files, Course);

            return RedirectToPage("./Index");
        }

         public async Task<IActionResult> OnPostDeleteFileAsync(int fileId)
        {
            var file = await _fileService.GetCourseDocumentAsync(fileId);

            if (file == null)
            {
                return NotFound();
            }

            if (file != null)
            {
                await _fileService.DeleteFileAsync(file);
            }

            return RedirectToPage("./Index");
        }
    }
}
