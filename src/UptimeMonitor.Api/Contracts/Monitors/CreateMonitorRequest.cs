using System.ComponentModel.DataAnnotations;

namespace UptimeMonitor.Api.Contracts.Monitors;

public sealed record CreateMonitorRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Endpoint { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CheckIntervalSeconds { get; init; } = 60;

    [Range(1, int.MaxValue)]
    public int TimeoutSeconds { get; init; } = 10;

    [Range(100, 599)]
    public int ExpectedStatusCode { get; init; } = 200;

    public bool IsEnabled { get; init; } = true;
}