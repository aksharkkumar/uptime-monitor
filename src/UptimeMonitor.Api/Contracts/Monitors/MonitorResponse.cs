namespace UptimeMonitor.Api.Contracts.Monitors;

public sealed record MonitorResponse(
    Guid Id,
    string Name,
    string Endpoint,
    int CheckIntervalSeconds,
    int TimeoutSeconds,
    int ExpectedStatusCode,
    bool IsEnabled,
    DateTime CreatedAt,
    DateTime UpdatedAt
);