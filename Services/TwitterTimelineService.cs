using Modisette.Pages;
using Newtonsoft.Json;

namespace Modisette.Services;

public class TwitterTimelineService : ITwitterTimelineService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TwitterTimelineService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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