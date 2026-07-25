using UptimeMonitor.Api;

namespace UptimeMonitor.UnitTests;

public sealed class ServiceStatusTests
{
    [Fact]
    public void ServiceStatusPreservesTheCurrentServiceState()
    {
        var timestamp = DateTimeOffset.UtcNow;

        var status = new ServiceStatus("uptime-monitor-api", "ready", timestamp);

        Assert.Equal("uptime-monitor-api", status.Service);
        Assert.Equal("ready", status.Status);
        Assert.Equal(timestamp, status.Timestamp);
    }
}
