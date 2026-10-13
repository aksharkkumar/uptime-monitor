using System.ComponentModel.DataAnnotations;

namespace UptimeMonitor.Api.Contracts.Monitors;

public sealed record UpdateMonitorRequest
{
    [Required]
    [StringLength(200)]
    public required string Name { get; init; }

    [Required]
    public required string Endpoint { get; init; }

    [Range(1, int.MaxValue)]
    public required int CheckIntervalSeconds { get; init; }

    [Range(1, int.MaxValue)]
    public required int TimeoutSeconds { get; init; }

    [Range(100, 599)]
    public required int ExpectedStatusCode { get; init; }

    public required bool IsEnabled { get; init; }

}