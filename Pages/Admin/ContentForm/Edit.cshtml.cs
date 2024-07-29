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

namespace modisette.Pages.Admin.ContentForm
{
    public class EditModel : PageModel
    {
        private readonly Modisette.Data.SiteContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(Modisette.Data.SiteContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            var course =  await _context.Courses.FirstOrDefaultAsync(m => m.Code == id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;

            Documents = await _context.CourseDocuments.Where(cd => cd.CourseCode == course.Code && 
                                                                   cd.CourseYear == course.Year && 
                                                                   cd.CourseSemester == course.Semester
                                                            ).ToListAsync();
            
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

            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await OnPostUploadAsync(Course);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.Code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CourseExists(string id)
        {
            return _context.Courses.Any(e => e.Code == id);
        }

        public async Task OnPostUploadAsync(Course course)
        {
            foreach (var formFile in Files.FormFiles)
            {
                if (formFile.Length > 0)
                {
                    var fileName = Path.GetFileName(formFile.FileName);
                    // var trustedFileNameForFileStorage = Path.GetRandomFileName();
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var fileUri = "/" + fileName; // Store the URI relative to the root

                    var courseDocument = new CourseDocument
                    {
                        CourseCode = course.Code,
                        CourseYear = course.Year,
                        CourseSemester = course.Semester,
                        Name = fileName,
                        Document = new Uri(fileUri, UriKind.Relative)
                    };

                    _context.CourseDocuments.Add(courseDocument);
                }
            }

            await _context.SaveChangesAsync();

        }

         public async Task<IActionResult> OnPostDeleteFileAsync(int fileId)
        {
            var file = await _context.CourseDocuments.FindAsync(fileId);

            if (file == null)
            {
                return NotFound();
            }

            if (file != null)
            {
                _context.CourseDocuments.Remove(file);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    

        public class BufferedFiles
        {
            [Required]
            public List<IFormFile> FormFiles { get; set; }
        }
    }
}
