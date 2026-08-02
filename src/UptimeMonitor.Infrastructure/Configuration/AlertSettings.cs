using System.ComponentModel.DataAnnotations;

namespace UptimeMonitor.Infrastructure.Configuration;
public sealed class AlertSettings
{
    public const string SectionName = "AlertSettings";
    public bool Enabled { get; init; } = true;

    [Range(1,100)]
    public int FailureThreshold { get; init; } = 3;

    [Range(0, 86_400)]
    public int CooldownSeconds { get; init; } = 300;
}