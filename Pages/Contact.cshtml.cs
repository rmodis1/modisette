using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Services;

namespace Modisette.Pages;

public class ContactModel : PageModel
{
    // Dependency Inversion Principle (DIP): Depend on abstractions (interfaces) rather than concrete implementations.
    private readonly Modisette.Data.SiteContext _context;
    private readonly IEmailService _emailService;
    private readonly IContactMessageBuilder _contactMessageBuilder;

    // Constructor Injection: Dependencies are injected through the constructor, promoting loose coupling.
    public ContactModel(Modisette.Data.SiteContext context, IEmailService emailService, IContactMessageBuilder contactMessageBuilder)
    {
        _context = context;
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

        _context.Contact.Add(Contact);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}