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


namespace Modisette.Pages.ContactForm
{
    public class DisplayModel : PageModel
    {
        private readonly IContactService _contactService;

        // The constructor demonstrates Dependency Injection, adhering to the Dependency Inversion Principle (DIP).
        // It depends on an abstraction (IContactService) rather than a concrete implementation.
        public DisplayModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IList<Contact> Contacts { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set;}

        // The OnGetAsync method follows the Interface Segregation Principle (ISP) by using a specific method from the IContactService interface.
        // This method also adheres to the Single Responsibility Principle (SRP) by focusing solely on fetching and setting the contacts.
        public async Task OnGetAsync()
        {
            Contacts = await _contactService.GetFilteredContactsAsync(SearchString);
        }

        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}
