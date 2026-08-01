namespace UptimeMonitor.Domain.MonitorChecks
{
    public sealed class MonitorCheck
    {
        // A monitor check represents a single check performed on a monitor endpoint. 
        // It contains information about the check, such as the monitor ID, timestamp, 
        // response time, status code, and other relevant properties.
        public MonitorCheck(Guid id, Guid monitorId, DateTime timestamp, int responseTimeMs, int statusCode, int expectedStatusCode)
        {
            if(id == Guid.Empty) throw new ArgumentException("Check ID cannot be empty.", nameof(id));
            if(monitorId == Guid.Empty) throw new ArgumentException("Monitor ID cannot be empty.", nameof(monitorId));
            if(responseTimeMs < 0) throw new ArgumentOutOfRangeException(nameof(responseTimeMs), "Response time cannot be negative.");
            if(statusCode < 100 || statusCode > 599) throw new ArgumentOutOfRangeException(nameof(statusCode), "Status code must be a valid HTTP status code.");
            if(expectedStatusCode < 100 || expectedStatusCode > 599) throw new ArgumentOutOfRangeException(nameof(expectedStatusCode), "Expected status code must be a valid HTTP status code.");

            Id = id;
            MonitorId = monitorId;
            CheckedAt = timestamp;
            LatencyMs = responseTimeMs;
            StatusCode = statusCode;
            ExpectedStatusCode = expectedStatusCode;
        }

        public Guid Id { get; }
        public Guid MonitorId { get; }
        public DateTime CheckedAt { get; }
        public int LatencyMs { get; }
        public int StatusCode { get; }
        public int ExpectedStatusCode { get; }  // Snapshot of expectation when check ran. 
        public bool IsSuccess => StatusCode == ExpectedStatusCode; // Derived from the monitor’s expected status rather than manually set.
    }
}