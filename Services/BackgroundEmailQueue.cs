using System.Threading.Channels;
using Modisette.Models;

namespace Modisette.Services;

public class BackgroundEmailQueue : IBackgroundEmailQueue
{
    private readonly Channel<EmailMessage> _queue = Channel.CreateUnbounded<EmailMessage>();

    public ValueTask QueueAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        return _queue.Writer.WriteAsync(message, cancellationToken);
    }

    public ValueTask<EmailMessage> DequeueAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAsync(cancellationToken);
    }
}