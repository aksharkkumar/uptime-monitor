using Microsoft.EntityFrameworkCore;
using UptimeMonitor.Domain.MonitorChecks;
using UptimeMonitor.Domain.MonitorEvents;
using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.Infrastructure.Persistence;

public sealed class UptimeMonitorDbContext(
    DbContextOptions<UptimeMonitorDbContext> options) : DbContext(options)
{
    public DbSet<EndpointMonitor> Monitors => Set<EndpointMonitor>();
    public DbSet<MonitorCheck> MonitorChecks => Set<MonitorCheck>();
    public DbSet<MonitorEvent> MonitorEvents => Set<MonitorEvent>();
}
