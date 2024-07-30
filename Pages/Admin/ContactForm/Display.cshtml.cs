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
        private readonly Modisette.Data.SiteContext _context;

        public DisplayModel(Modisette.Data.SiteContext context)
        {
            _context = context;
        }

        public IList<Contact> Contacts { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set;}

        public async Task OnGetAsync()
        {
            var contacts = from contact in _context.Contact
                           select contact;
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                contacts = contacts.Where(contact => contact.FirstName.Contains(SearchString) || 
                                                     contact.LastName.Contains(SearchString) || 
                                                     contact.Message.Contains(SearchString)||
                                                     contact.Notes.Contains(SearchString));
            }

            Contacts = await contacts.ToListAsync();
        }

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
