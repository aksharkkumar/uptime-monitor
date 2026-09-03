using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.Infrastructure.Persistence;

internal sealed class EndpointMonitorConfiguration : IEntityTypeConfiguration<EndpointMonitor>
{
    public void Configure(EntityTypeBuilder<EndpointMonitor> builder)
    {
        builder.ToTable("Monitors");

        builder.HasKey(monitor => monitor.Id);
        builder.Property(monitor => monitor.Id).ValueGeneratedNever();

        builder.Property(monitor => monitor.Name).IsRequired();
        builder.Property(monitor => monitor.Endpoint)
            .HasConversion(
                endpoint => endpoint.ToString(),
                value => new Uri(value, UriKind.Absolute))
            .IsRequired();
    }
}
