using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Data;
using Modisette.Services;

namespace modisette.Pages.ContactForm
{
    public class DeleteModel : PageModel
    {
        private readonly IContactService _contactService;

        public DeleteModel(IContactService contactService)
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

            var contact = await _contactService.GetContactByIdAsync(id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact != null)
            {
                Contact = contact;
                await _contactService.DeleteContactAsync(Contact);
            }

            return RedirectToPage("./Display");
        }

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
