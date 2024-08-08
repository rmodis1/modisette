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
        // Dependency Inversion Principle (DIP): the EditModel class depends on abstractions (ICourseService and IFileService) rather than concrete implementations.
        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

        // Constructor Injection: this follows the Dependency Inversion Principle (DIP) by injecting the ICourseService and IFileService dependencies through the constructor.
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

        // Single Responsibility Principle (SRP): the OnGetAsync method is responsible only for handling GET requests and retrieving the course and its documents by its id/code.
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Delegates the responsibility of retrieving a course to the ICourseService.
            var course =  await _courseService.GetCourseByCodeAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            Course = course;

            // Delegates the responsibility of retrieving course documents to the IFileService.
            Documents = await _fileService.GetCourseDocumentsAsync(course);
            
            return Page();
        }

        // Consider if overposting attacks are a concern.
        // Single Responsibility Principle (SRP): the OnPostAsync method is responsible only for handling POST requests and updating the course and its files.
        // Open/Closed Principle (OCP): the method is open for extension (e.g., adding more logic) but closed for modification (the core logic remains unchanged).
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Delegates the responsibility of updating a course to the ICourseService.
            var updateResult = await _courseService.UpdateCourseAsync(Course);

            if (!updateResult)
            {
                return NotFound();
            }

            // Delegates the responsibility of uploading files to the IFileService.
            await _fileService.UploadFilesAsync(Files, Course);

            return RedirectToPage("./Index");
        }

        // Single Responsibility Principle (SRP): the OnPostDeleteFileAsync method is responsible only for handling POST requests and deleting a file.
         public async Task<IActionResult> OnPostDeleteFileAsync(int fileId)
        {
            // Delegates the responsibility of retrieving a course document to the IFileService.
            var file = await _fileService.GetCourseDocumentAsync(fileId);

            if (file == null)
            {
                return NotFound();
            }

            if (file != null)
            {
                // Delegates the responsibility of deleting a file to the IFileService.
                await _fileService.DeleteFileAsync(file);
            }

            return RedirectToPage("./Index");
        }
    }
}
