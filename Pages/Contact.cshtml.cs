using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Repositories;
using Modisette.Services;

namespace Modisette.Pages;

public class ContactModel : PageModel
{
    // Dependency Inversion Principle (DIP): Depend on abstractions (interfaces) rather than concrete implementations.
    private readonly IContactService _contactService;
    private readonly IEmailService _emailService;
    private readonly IContactMessageBuilder _contactMessageBuilder;

    // Constructor Injection: Dependencies are injected through the constructor, promoting loose coupling.
    public ContactModel(IContactService contactService, IEmailService emailService, IContactMessageBuilder contactMessageBuilder)
    {
        _contactService = contactService;
        _emailService = emailService;
        _contactMessageBuilder = contactMessageBuilder;
    }

    [BindProperty]
    public Contact Contact { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Single Responsibility Principle (SRP): Delegates the message building responsibility to the IContactMessageBuilder service.
        // Creates an email message from the contact form data.
        EmailMessage messageToSend = _contactMessageBuilder.BuildMessage(Contact);

        try
        {
            // Single Responsibility Principle (SRP): Delegates the email sending responsibility to the IEmailService service.
            await _emailService.Send(messageToSend);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // Adds the contact form data to the database.
        await _contactService.CreateContactAsync(Contact);

        return RedirectToPage("./Index");
    }
}