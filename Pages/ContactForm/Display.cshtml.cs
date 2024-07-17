using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Data;

namespace Modisette.Pages.ContactForm
{
    public class DisplayModel : PageModel
    {
        private readonly Modisette.Data.ContactFormContext _context;

        public DisplayModel(Modisette.Data.ContactFormContext context)
        {
            _context = context;
        }

        public IList<Contact> Contact { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Contact = await _context.Contact.ToListAsync();
        }
    }
}
