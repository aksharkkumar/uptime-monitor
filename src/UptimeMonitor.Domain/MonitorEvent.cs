namespace UptimeMonitor.Domain.MonitorEvents
{
    public sealed class MonitorEvent
    {
        #region Constructors
        // A monitor event represents a notification that is triggered when a monitor check fails or meets certain conditions. 
        // It contains information about the event, such as the monitor ID, timestamp, event type, and other relevant properties.
        public MonitorEvent(Guid id, Guid monitorId, Guid? triggeringCheckId, DateTime timestamp, MonitorEventType type)
        {
            Id = id;
            MonitorId = monitorId;
            TriggeringCheckId = triggeringCheckId;
            TriggeredAt = timestamp;
            Type = type;
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public Guid MonitorId { get; set; }
        public Guid? TriggeringCheckId { get; set; } // Optional reference to the monitor check that triggered this event
        public DateTime TriggeredAt { get; set; }
        public MonitorEventType Type { get; set; } // e.g., "DOWN", "SLOW_RESPONSE", etc.
        public string? Message { get; set; } // Optional message providing additional context about the alert
        public bool IsResolved => ResolvedAt.HasValue; // Indicates whether the alert has been resolved
        public DateTime? ResolvedAt { get; private set; } // Timestamp when the alert was resolved, if applicable
        #endregion

        #region Methods
        public void Resolve(DateTime resolvedAt)
        {
            if (IsResolved)
                return; // Idempotent: if already resolved, do nothing

            if (resolvedAt < TriggeredAt)
            {
                throw new ArgumentException("Resolved timestamp cannot precede the triggered timestamp.", nameof(resolvedAt));
            }
            ResolvedAt = resolvedAt;
        }
        #endregion
    }
}