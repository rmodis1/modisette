using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;
using Modisette.Services;

namespace modisette.Pages.Admin.ContentForm
{
    public class DetailsModel : PageModel
    {
        // Dependency Inversion Principle (DIP): the DeleteModel class depends on abstractions (ICourseService, IFileService) rather than concrete implementations.
        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

        // Constructor Injection: this follows the Dependency Inversion Principle (DIP) by injecting the ICourseService dependency through the constructor.
        public DetailsModel(ICourseService courseService, IFileService fileService)
        {
            _courseService = courseService;
            _fileService = fileService;
        }

        public IList<CourseDocument> Documents { get; set; } = default!;
        public Course Course { get; set; } = default!;

        // Single Responsibility Principle (SRP): the OnGetAsync method is responsible only for handling GET requests and retrieving the course by its id/code.
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Delegates the responsibility of retrieving a course to the ICourseService.
            var course = await _courseService.GetCourseByCodeAsync(id);
   
            if (course == null)
            {
                return NotFound();
            }
            else
            {
                Course = course;
            }

            // Delegates the responsibility of retrieving course documents to the IFileService.
            Documents = await _fileService.GetCourseDocumentsAsync(course);
            
            return Page();
        }
    }
}
