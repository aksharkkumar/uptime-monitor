using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UptimeMonitor.Domain.MonitorChecks;
using UptimeMonitor.Domain.MonitorEvents;
using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.Infrastructure.Persistence;

internal sealed class MonitorEventConfiguration : IEntityTypeConfiguration<MonitorEvent>
{
    public void Configure(EntityTypeBuilder<MonitorEvent> builder)
    {
        builder.ToTable("MonitorEvents");

        builder.HasKey(monitorEvent => monitorEvent.Id);
        builder.Property(monitorEvent => monitorEvent.Id).ValueGeneratedNever();
        builder.Property(monitorEvent => monitorEvent.Type)
            .HasColumnType("monitor_event_type");
        builder.Ignore(monitorEvent => monitorEvent.IsResolved);

        builder.HasOne<EndpointMonitor>()
            .WithMany()
            .HasForeignKey(monitorEvent => monitorEvent.MonitorId)
            .IsRequired();

        builder.HasOne<MonitorCheck>()
            .WithMany()
            .HasForeignKey(monitorEvent => monitorEvent.TriggeringCheckId)
            .IsRequired(false);
    }
}
