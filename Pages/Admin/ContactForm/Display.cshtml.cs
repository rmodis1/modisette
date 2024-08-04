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

namespace Modisette.Pages.ContactForm
{
    public class DisplayModel : PageModel
    {
        private readonly IContactService _contactService;

        public DisplayModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IList<Contact> Contacts { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set;}

        public async Task OnGetAsync()
        {
            Contacts = await _contactService.GetFilteredContactsAsync(SearchString);
        
        }

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
