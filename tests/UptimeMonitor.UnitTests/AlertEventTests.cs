using UptimeMonitor.Domain.AlertEvents;

namespace UptimeMonitor.UnitTests;
public sealed class AlertEventTests
{
    [Fact]
    public void AlertEventPreservesTheCurrentAlertEventState()
    {
        var alertId = Guid.NewGuid();
        var monitorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var alertType = "DOWN";
        var alertEvent = new AlertEvent(alertId, monitorId, timestamp, alertType);

        Assert.Equal(alertId, alertEvent.Id);
        Assert.Equal(monitorId, alertEvent.MonitorId);
        Assert.Equal(timestamp, alertEvent.TriggeredAt);
        Assert.Equal(alertType, alertEvent.AlertType);
    }

    [Fact]
    public void AlertEventCreatesUnresolvedAlertEvent()
    {
        var alertId = Guid.NewGuid();
        var monitorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var alertType = "SLOW_RESPONSE";
        var alertEvent = new AlertEvent(alertId, monitorId, timestamp, alertType);

        Assert.False(alertEvent.IsResolved);
        Assert.Null(alertEvent.ResolvedAt);
    }

    /*
    AlertEvent rejects blank or unsupported alert types.
    Alert types use an enum or value object instead of arbitrary strings.
    Resolving an alert sets IsResolved and ResolvedAt together.
    Resolving an already resolved alert is idempotent or explicitly rejected.
    Alert resolution timestamps cannot precede TriggeredAt
    */
}