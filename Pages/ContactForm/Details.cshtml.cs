using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Data;

namespace modisette.Pages.ContactForm
{
    public class DetailsModel : PageModel
    {
        private readonly Modisette.Data.SiteContext _context;

        public DetailsModel(Modisette.Data.SiteContext context)
        {
            _context = context;
        }

        public Contact Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            else
            {
                Contact = contact;
            }
            return Page();
        }

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
