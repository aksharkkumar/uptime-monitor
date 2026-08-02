namespace UptimeMonitor.Domain.Monitors
{
    public sealed class EndpointMonitor
    {
        #region Constructors
        // A monitor represents a single endpoint that is being monitored for uptime and performance.
        // It contains information about the endpoint, such as its name, URL, check interval, timeout,
        // expected status code, and other relevant properties.
        public EndpointMonitor(Guid id, string name, Uri endpoint)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNull(endpoint);
            
            Id = id;
            Name = name;
            Endpoint = endpoint;

            DateTime utcNow = DateTime.UtcNow;
            CreatedAt = utcNow;
            UpdatedAt = utcNow;
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Endpoint { get; set; }
        public int CheckIntervalSeconds { get; set; } // in seconds
        public int TimeoutSeconds { get; set; } // in seconds
        public int ExpectedStatusCode { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; set; }
        #endregion
    }
}