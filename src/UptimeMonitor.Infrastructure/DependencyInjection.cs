using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UptimeMonitor.Infrastructure.Configuration;

namespace UptimeMonitor.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: Add InvalidOperationException once DB persistence is added   
        var connectionString = configuration.GetConnectionString("UptimeMonitor"); 
        
        services
            .AddOptions<AlertSettings>()
            .Bind(configuration.GetSection(AlertSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // TODO: Add database registration when DbContext exists
        // services.AddDbContext<UptimeMonitorDbContext>(options => 
        //      options.UseNpgsql(connectionString));

        return services;
    }
}