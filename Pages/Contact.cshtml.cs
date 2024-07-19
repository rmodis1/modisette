using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;


namespace Modisette.Pages;

public class ContactModel : PageModel
{
    private readonly Modisette.Data.ContactFormContext _context;
    private readonly EmailAddress _FromAndToEmailAddress;
    private readonly IEmailService _EmailService;

    public ContactModel(Modisette.Data.ContactFormContext context, EmailAddress fromAndToEmailAddress, IEmailService emailService)
    {
        _context = context;
        _FromAndToEmailAddress = fromAndToEmailAddress;
        _EmailService = emailService;
    }


    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Contact Contact { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        EmailMessage messageToSend = new EmailMessage
        {
            FromEmailAddress = new List<EmailAddress> { _FromAndToEmailAddress },
            ToEmailAddress = new List<EmailAddress> { _FromAndToEmailAddress },
            Content = $"Someone just contacted you through your website!\n" +
            $"Name: {Contact.FirstName} {Contact.LastName}\n" +
            $"Email: {Contact.Email}\n" +
            $"Message: {Contact.Message}",
            Subject = "Contact Form"
        };
        try
        {
            await _EmailService.Send(messageToSend);
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