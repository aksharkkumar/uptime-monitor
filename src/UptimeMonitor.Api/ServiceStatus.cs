namespace UptimeMonitor.Api;

public sealed record ServiceStatus(string Service, string Status, DateTimeOffset Timestamp);
