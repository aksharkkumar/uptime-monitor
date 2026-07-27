namespace UptimeMonitor.Domain.AlertEvents
{
    public sealed class AlertEvent
    {
        // An alert event represents a notification that is triggered when a monitor check fails or meets certain conditions. 
        // It contains information about the alert, such as the monitor ID, timestamp, alert type, and other relevant properties.
        public AlertEvent(Guid id, Guid monitorId, DateTime timestamp, string alertType)
        {
            Id = id;
            MonitorId = monitorId;
            TriggeredAt = timestamp;
            AlertType = alertType;
        }

        public Guid Id { get; set; }
        public Guid MonitorId { get; set; }
        public DateTime TriggeredAt { get; set; }
        public string AlertType { get; set; } // e.g., "DOWN", "SLOW_RESPONSE", etc.
        public string? Message { get; set; } // Optional message providing additional context about the alert
        public bool IsResolved { get; private set; } // Indicates whether the alert has been resolved or acknowledged
        public DateTime? ResolvedAt { get; private set; } // Timestamp when the alert was resolved, if applicable
    }
}