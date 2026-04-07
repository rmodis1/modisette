using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Modisette.Pages;

public class IndexModel : PageModel
{
    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet()
    {
        // The home page is static aside from optional TempData status messages.
    }
}
