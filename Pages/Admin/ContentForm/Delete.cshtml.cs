using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace modisette.Pages.Admin.ContentForm
{
    public class DeleteModel : PageModel
    {
        private readonly Modisette.Data.SiteContext _context;

        public DeleteModel(Modisette.Data.SiteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.Code == id);

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

            var courseToDelete = await _context.Courses.SingleOrDefaultAsync(m => m.Code == Course.Code &&
                                                                                  m.Year == Course.Year &&
                                                                                  m.Semester == Course.Semester);
            if (courseToDelete != null)
            {
                _context.Courses.Remove(courseToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
