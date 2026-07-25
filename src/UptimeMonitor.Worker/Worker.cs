namespace UptimeMonitor.Worker;

public partial class Worker(ILogger<Worker> logger) : BackgroundService
{
    private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            LogHeartbeat(logger, DateTimeOffset.UtcNow);
            await Task.Delay(HeartbeatInterval, stoppingToken);
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Uptime monitoring worker heartbeat at {Timestamp}")]
    private static partial void LogHeartbeat(ILogger logger, DateTimeOffset timestamp);
}
