// POST /monitors -> Create a monitor; return 201 Created with its location

// GET /monitors -> List configured monitors

// GET /monitors/{id} -> Fetch one monitor; return 404 if missing

using UptimeMonitor.Api.Contracts.Monitors;
using UptimeMonitor.Domain.Monitors;

namespace UptimeMonitor.Api.Endpoints;

public static class MonitorEndpoints
{
    public static IEndpointRouteBuilder MapMonitorEndpoints (
        this IEndpointRouteBuilder app
    )
    {
        var group = app.MapGroup("/monitors");
        
        // register route handlers here.

        return app;
    }

    private static MonitorResponse ToResponse(EndpointMonitor monitor) => 
        new (
            monitor.Id,
            monitor.Name,
            monitor.Endpoint.ToString(),
            monitor.CheckIntervalSeconds,
            monitor.TimeoutSeconds,
            monitor.ExpectedStatusCode,
            monitor.IsEnabled,
            monitor.CreatedAt,
            monitor.UpdatedAt
        );
}