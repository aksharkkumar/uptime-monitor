using UptimeMonitor.Domain.MonitorChecks;

namespace UptimeMonitor.UnitTests;
public sealed class MonitorCheckTests
{
    [Fact]
    public void MonitorCheckPreservesTheCurrentMonitorCheckState()
    {
        var checkId = Guid.NewGuid();
        var monitorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var responseTimeMs = 150;
        var statusCode = 200;

        var monitorCheck = new MonitorCheck(checkId, monitorId, timestamp, responseTimeMs, statusCode);
        
        Assert.Equal(checkId, monitorCheck.Id);
        Assert.Equal(monitorId, monitorCheck.MonitorId); 
        Assert.Equal(timestamp, monitorCheck.CheckedAt);
        Assert.Equal(responseTimeMs, monitorCheck.LatencyMs);
        Assert.Equal(statusCode, monitorCheck.StatusCode);
    }

    /*
    MonitorCheck rejects empty IDs, negative latency, or invalid status codes.
    MonitorCheck.IsSuccess is derived from the monitor’s expected status rather than manually set.
    */
}