using Modisette.Models;

namespace Modisette.Services;

public interface IBackgroundEmailQueue
{
    ValueTask QueueAsync(EmailMessage message, CancellationToken cancellationToken = default);

    ValueTask<EmailMessage> DequeueAsync(CancellationToken cancellationToken);
}