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
        private readonly ICourseService _courseService;

        public DeleteModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        public async Task<IActionResult> OnPostAsync(Course course)
        {
            if (course == null)
            {
                return NotFound();
            }

            var courseToDelete = await _courseService.GetCourseAsync(course);

            if (courseToDelete != null)
            {
                await _courseService.DeleteCourseAsync(courseToDelete);
            }

            return RedirectToPage("./Index");
        }
    }
}
