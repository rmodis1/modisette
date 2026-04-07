using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Services;

namespace Modisette.Pages;

public class ContactModel : PageModel
{
    // Dependency Inversion Principle (DIP): Depend on abstractions (interfaces) rather than concrete implementations.
    private readonly IContactService _contactService;
    private readonly IBackgroundEmailQueue _backgroundEmailQueue;
    private readonly IContactMessageBuilder _contactMessageBuilder;
    private readonly ILogger<ContactModel> _logger;

    // Constructor Injection: Dependencies are injected through the constructor, promoting loose coupling.
    public ContactModel(
        IContactService contactService,
        IBackgroundEmailQueue backgroundEmailQueue,
        IContactMessageBuilder contactMessageBuilder,
        ILogger<ContactModel> logger)
    {
        _contactService = contactService;
        _backgroundEmailQueue = backgroundEmailQueue;
        _contactMessageBuilder = contactMessageBuilder;
        _logger = logger;
    }

    [TempData]
    public string? StatusMessage { get; set; }

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
            await _backgroundEmailQueue.QueueAsync(messageToSend);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to queue contact form notification email.");
        }

        StatusMessage = "Thanks for reaching out. Your message has been received.";

        return RedirectToPage("./Index");
    }
}