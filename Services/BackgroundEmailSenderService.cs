using Microsoft.Extensions.Hosting;
using Modisette.Models;

namespace Modisette.Services;

public class BackgroundEmailSenderService : BackgroundService
{
    private readonly IBackgroundEmailQueue _backgroundEmailQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundEmailSenderService> _logger;

    public BackgroundEmailSenderService(
        IBackgroundEmailQueue backgroundEmailQueue,
        IServiceProvider serviceProvider,
        ILogger<BackgroundEmailSenderService> logger)
    {
        _backgroundEmailQueue = backgroundEmailQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            EmailMessage message;

            try
            {
                message = await _backgroundEmailQueue.DequeueAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send queued contact form notification email.");
            }
        }
    }
}