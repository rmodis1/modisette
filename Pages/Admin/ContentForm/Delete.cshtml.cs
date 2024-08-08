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
    public class DeleteModel : PageModel
    {
        // Dependency Inversion Principle (DIP): the DeleteModel class depends on an abstraction (ICourseService) rather than a concrete implementation.
        private readonly ICourseService _courseService;

        // Constructor Injection: this follows the Dependency Inversion Principle (DIP) by injecting the ICourseService dependency through the constructor.
        public DeleteModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
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
            return Page();
        }

        // Single Responsibility Principle (SRP): the OnPostAsync method is responsible only for handling POST requests and deleting the course.
        // Open/Closed Principle (OCP): the method is open for extension (e.g., adding more logic) but closed for modification (the core logic remains unchanged).
        public async Task<IActionResult> OnPostAsync(Course course)
        {
            if (course == null)
            {
                return NotFound();
            }

            // Delegates the responsibility of retrieving a course to the ICourseService.
            var courseToDelete = await _courseService.GetCourseAsync(course);

            if (courseToDelete != null)
            {
                // Delegates the responsibility of deleting a course to the ICourseService.
                await _courseService.DeleteCourseAsync(courseToDelete);
            }

            return RedirectToPage("./Index");
        }
    }
}
