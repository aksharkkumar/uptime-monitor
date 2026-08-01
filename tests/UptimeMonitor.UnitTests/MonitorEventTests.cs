using UptimeMonitor.Domain;
using UptimeMonitor.Domain.MonitorEvents;

namespace UptimeMonitor.UnitTests;
public sealed class MonitorEventTests
{
    [Fact]
    public void MonitorEventPreservesItsInitialState()
    {
        var eventId = Guid.NewGuid();
        var monitorId = Guid.NewGuid();
        var triggeringCheckId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var type = MonitorEventType.MonitorDown;
        var monitorEvent = new MonitorEvent(eventId, monitorId, triggeringCheckId, timestamp, type);

        Assert.Equal(eventId, monitorEvent.Id);
        Assert.Equal(monitorId, monitorEvent.MonitorId);
        Assert.Equal(triggeringCheckId, monitorEvent.TriggeringCheckId);
        Assert.Equal(timestamp, monitorEvent.TriggeredAt);
        Assert.Equal(type, monitorEvent.Type);
    }

    [Fact]
    public void MonitorEventAllowsNoTriggeringCheck()
    {
        var monitorEvent = CreateMonitorEvent(triggeringCheckId: null);

        Assert.Null(monitorEvent.TriggeringCheckId);
    }

    [Fact]
    public void MonitorEventStartsUnresolved()
    {
        var monitorEvent = CreateMonitorEvent();

        Assert.False(monitorEvent.IsResolved);
        Assert.Null(monitorEvent.ResolvedAt);
    }

    [Fact]
    public void ResolveSetsResolutionStateAndTimestamp()
    {
        var triggeredAt = DateTime.UtcNow;
        var resolvedAt = triggeredAt.AddMinutes(1);
        var monitorEvent = CreateMonitorEvent(triggeredAt: triggeredAt);

        monitorEvent.Resolve(resolvedAt);

        Assert.True(monitorEvent.IsResolved);
        Assert.Equal(resolvedAt, monitorEvent.ResolvedAt);
    }

    [Fact]
    public void ResolveIsIdempotent()
    {
        var triggeredAt = DateTime.UtcNow;
        var firstResolution = triggeredAt.AddMinutes(1);
        var laterResolution = triggeredAt.AddMinutes(2);
        var monitorEvent = CreateMonitorEvent(triggeredAt: triggeredAt);

        monitorEvent.Resolve(firstResolution);
        monitorEvent.Resolve(laterResolution);

        Assert.True(monitorEvent.IsResolved);
        Assert.Equal(firstResolution, monitorEvent.ResolvedAt);
    }

    [Fact]
    public void ResolveRejectsTimestampBeforeEventWasTriggered()
    {
        var triggeredAt = DateTime.UtcNow;
        var monitorEvent = CreateMonitorEvent(triggeredAt: triggeredAt);

        var exception = Assert.Throws<ArgumentException>(
            () => monitorEvent.Resolve(triggeredAt.AddTicks(-1)));

        Assert.Equal("resolvedAt", exception.ParamName);
        Assert.False(monitorEvent.IsResolved);
        Assert.Null(monitorEvent.ResolvedAt);
    }

    private static MonitorEvent CreateMonitorEvent(
        Guid? triggeringCheckId = null,
        DateTime? triggeredAt = null)
    {
        return new MonitorEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            triggeringCheckId,
            triggeredAt ?? DateTime.UtcNow,
            MonitorEventType.SlowResponse);
    }
}
