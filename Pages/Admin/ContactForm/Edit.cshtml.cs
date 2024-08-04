using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Data;
using Modisette.Repositories;

namespace modisette.Pages.ContactForm
{
    public class EditModel : PageModel
    {
        private readonly IContactService _contactService;

        public EditModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        [BindProperty]
        public Contact Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact =  await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            Contact = contact;
            return Page();
        }

        // Perhaps add protection against overposting attacks.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _contactService.UpdateContactAsync(Contact);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _contactService.ContactExistsAsync(Contact.Id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Display");
        }
    }
}
