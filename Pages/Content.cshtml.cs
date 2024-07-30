using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Modisette.Pages;

public class ContentModel : PageModel
{
    private readonly ILogger<ContentModel> _logger;

    public ContentModel(ILogger<ContentModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

