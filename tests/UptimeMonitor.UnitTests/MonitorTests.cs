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

    [Fact]
    public void EndpointMonitorCreatedAtAndUpdatedAtAreSetToCurrentUtcTime()
    {
        var monitorId = Guid.NewGuid();
        var monitorName = "Test Monitor";
        var monitorEndpoint = new Uri("https://example.com");

        var beforeCreation = DateTime.UtcNow;
        var monitor = new EndpointMonitor(monitorId, monitorName, monitorEndpoint);
        var afterCreation = DateTime.UtcNow;

        Assert.InRange(monitor.UpdatedAt, beforeCreation, afterCreation);
        Assert.InRange(monitor.CreatedAt, beforeCreation, afterCreation);
        Assert.Equal(monitor.CreatedAt, monitor.UpdatedAt);
    }

    /*
    Monitor rejects Guid.Empty.
    Monitor accepts only absolute HTTP/HTTPS endpoints.
    Check interval and timeout must be positive.
    Expected status code must be within the valid HTTP range.
    Updating monitor configuration advances UpdatedAt.
    Enable/disable or activate/deactivate operations enforce valid state transitions. */

}