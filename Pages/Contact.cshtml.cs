using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;


namespace Modisette.Pages;

public class ContactModel : PageModel
{
    private readonly Modisette.Data.ContactFormContext _context;

    public ContactModel(Modisette.Data.ContactFormContext context)
    {
        _context = context;
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

        _context.Contact.Add(Contact);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}