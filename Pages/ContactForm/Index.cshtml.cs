using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace modisette.Pages.ContactForm
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGetPartial() =>
            Partial("_PartialAuthorization");
    }
}