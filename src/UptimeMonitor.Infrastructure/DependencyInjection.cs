using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UptimeMonitor.Infrastructure.Configuration;
using UptimeMonitor.Infrastructure.Persistence;

namespace UptimeMonitor.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(
        this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<UptimeMonitorDbContext>("UptimeMonitor");

        builder.Services
            .AddOptions<AlertSettings>()
            .Bind(builder.Configuration.GetSection(AlertSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }
}
