namespace Modisette.Services;

public interface ITimelineService
{
    Task<string> GetEmbeddedTimelineAsync(string url);
}