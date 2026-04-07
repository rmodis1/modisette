using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Services;

namespace Modisette.Pages;

public class ContactModel : PageModel
{
    private static readonly TimeSpan EmailSendTimeout = TimeSpan.FromSeconds(10);

    // Dependency Inversion Principle (DIP): Depend on abstractions (interfaces) rather than concrete implementations.
    private readonly IContactService _contactService;
    private readonly IEmailService _emailService;
    private readonly IContactMessageBuilder _contactMessageBuilder;
    private readonly ILogger<ContactModel> _logger;

    // Constructor Injection: Dependencies are injected through the constructor, promoting loose coupling.
    public ContactModel(
        IContactService contactService,
        IEmailService emailService,
        IContactMessageBuilder contactMessageBuilder,
        ILogger<ContactModel> logger)
    {
        _contactService = contactService;
        _emailService = emailService;
        _contactMessageBuilder = contactMessageBuilder;
        _logger = logger;
    }

    [BindProperty]
    public Contact Contact { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _contactService.CreateContactAsync(Contact);

        // Single Responsibility Principle (SRP): Delegates the message building responsibility to the IContactMessageBuilder service.
        // Creates an email message from the contact form data.
        EmailMessage messageToSend = _contactMessageBuilder.BuildMessage(Contact);

        try
        {
            // Single Responsibility Principle (SRP): Delegates the email sending responsibility to the IEmailService service.
            await _emailService.Send(messageToSend).WaitAsync(EmailSendTimeout);
        }
        catch (TimeoutException ex)
        {
            _logger.LogWarning(ex, "Timed out sending contact form notification email after {TimeoutSeconds} seconds.", EmailSendTimeout.TotalSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact form notification email.");
        }

        return RedirectToPage("./Index");
    }
}