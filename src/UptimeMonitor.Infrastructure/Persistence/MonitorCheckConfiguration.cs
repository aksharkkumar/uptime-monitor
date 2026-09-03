using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UptimeMonitor.Domain.MonitorChecks;
using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.Infrastructure.Persistence;

internal sealed class MonitorCheckConfiguration : IEntityTypeConfiguration<MonitorCheck>
{
    public void Configure(EntityTypeBuilder<MonitorCheck> builder)
    {
        builder.ToTable("MonitorChecks");

        builder.HasKey(check => check.Id);
        builder.Property(check => check.Id).ValueGeneratedNever();
        builder.Property(check => check.MonitorId);
        builder.Property(check => check.CheckedAt);
        builder.Property(check => check.LatencyMs);
        builder.Property(check => check.StatusCode);
        builder.Property(check => check.ExpectedStatusCode);
        builder.Ignore(check => check.IsSuccess);

        builder.HasOne<EndpointMonitor>()
            .WithMany()
            .HasForeignKey(check => check.MonitorId)
            .IsRequired();
    }
}
