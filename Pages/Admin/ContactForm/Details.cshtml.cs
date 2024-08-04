using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;
using Modisette.Data;
using Modisette.Repositories;

namespace modisette.Pages.ContactForm
{
    public class DetailsModel : PageModel
    {
        private readonly IContactService _contactService;

        public DetailsModel(IContactService contactService)
        {
            _contactService = contactService;
        }

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

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
