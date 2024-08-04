using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Modisette.Pages;
public class AboutModel : PageModel
{
    // The AboutModel class follows the Single Responsibility Principle (SRP) by focusing on handling the About page's data and behavior.
    private readonly ITimelineService _twitterTimelineService;

    // The constructor demonstrates Dependency Injection, adhering to the Dependency Inversion Principle (DIP).
    // It depends on an abstraction (ITimelineService) rather than a concrete implementation.
    public AboutModel(ITimelineService twitterTimelineService)
    {
        _twitterTimelineService = twitterTimelineService;
    }

    [BindProperty]
    public string EmbeddedTimelineHtml { get; private set; }

    // The OnGetAsync method follows the Interface Segregation Principle (ISP) by using a specific method from the ITimelineService interface.
    // This method also adheres to the SRP by focusing solely on fetching and setting the embedded timeline HTML.
    public async Task OnGetAsync()
    {
        // The use of ITimelineService here ensures that the AboutModel class is open for extension but closed for modification,
        // following the Open/Closed Principle (OCP). Any changes to the timeline fetching logic can be made in the service implementation
        // without modifying this class.
        EmbeddedTimelineHtml = await _twitterTimelineService.GetEmbeddedTimelineAsync("https://twitter.com/VickiModisette/status/1563983500755230720");
    }
}

// The TwitterOEmbedResponse class is a simple data transfer object (DTO) that follows the SRP by only containing data related to the Twitter OEmbed response.
public class TwitterOEmbedResponse
{
    public string? Html { get; set; }
}

