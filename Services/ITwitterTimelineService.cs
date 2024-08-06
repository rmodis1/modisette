namespace Modisette.Services;

public interface ITwitterTimelineService
{
    Task<string> GetEmbeddedTimelineAsync(string url);
}