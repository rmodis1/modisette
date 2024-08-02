using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace Modisette.Pages;
public class AboutModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public AboutModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public string EmbeddedTimelineHtml { get; private set; }

    public async Task OnGetAsync()
    {
        EmbeddedTimelineHtml = await GetEmbeddedTimelineAsync("https://twitter.com/VickiModisette");
    }

    public async Task<string> GetEmbeddedTimelineAsync(string url)
    {
        var requestUrl = $"https://publish.twitter.com/oembed?url={url}&omit_script=true";
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response 
                throw new HttpRequestException($"Request to Twitter oEmbed API failed with status code {response.StatusCode}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();

            string oEmbedResponseHtml;
            using (var jsonReader = new JsonTextReader(new StringReader(responseContent)))
            {
                var serializer = new JsonSerializer();
                var oEmbedResponse = serializer.Deserialize<TwitterOEmbedResponse>(jsonReader);
                oEmbedResponseHtml = oEmbedResponse.Html;
            }

            return oEmbedResponseHtml;
        }
        catch (HttpRequestException ex)
        {
            return $"An error occurred while fetching the Twitter timeline: {ex.Message}";
        }
    }
}

public class TwitterOEmbedResponse
{
    public string Html { get; set; }
}

