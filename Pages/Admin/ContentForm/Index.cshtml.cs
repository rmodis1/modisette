using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Services;

namespace modisette.Pages.Admin.ContentForm
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IList<Course> Course { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Course = await _courseService.GetCoursesAsync();
        }
    }
}
