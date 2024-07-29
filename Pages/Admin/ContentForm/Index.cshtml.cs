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
    public class IndexModel : PageModel
    {
        private readonly Modisette.Data.SiteContext _context;

        public IndexModel(Modisette.Data.SiteContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Course = await _context.Courses.ToListAsync();
        }
    }
}
