namespace UptimeMonitor.Domain.MonitorChecks
{
    public sealed class MonitorCheck
    {
        // A monitor check represents a single check performed on a monitor endpoint. 
        // It contains information about the check, such as the monitor ID, timestamp, 
        // response time, status code, and other relevant properties.
        public MonitorCheck(Guid id, Guid monitorId, DateTime timestamp, int responseTimeMs, int statusCode)
        {
            Id = id;
            MonitorId = monitorId;
            CheckedAt = timestamp;
            LatencyMs = responseTimeMs;
            StatusCode = statusCode;
        }

        public Guid Id { get; set; }
        public Guid MonitorId { get; set; }
        public DateTime CheckedAt { get; set; }
        public int LatencyMs { get; set; } // in milliseconds
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; } // Indicates whether the check was successful (e.g., status code 200)
    }
}