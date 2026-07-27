using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.UnitTests;
public sealed class EndpointMonitorTests
{
    [Fact]
    public void EndpointMonitorPreservesTheCurrentEndpointMonitorState()
    {
        var monitorId = Guid.NewGuid();
        var monitorName = "Test Monitor";
        var monitorEndpoint = new Uri("https://example.com");

        var monitor = new EndpointMonitor(monitorId, monitorName, monitorEndpoint);

        Assert.Equal(monitorId, monitor.Id);
        Assert.Equal(monitorName, monitor.Name);
        Assert.Equal(monitorEndpoint, monitor.Endpoint);
        Assert.Equal(DateTime.UtcNow.Date, monitor.CreatedAt.Date);
        Assert.Equal(DateTime.UtcNow.Date, monitor.UpdatedAt.Date);
    }
}