namespace UptimeMonitor.Domain.Monitors
{
    public sealed class Monitor
    {
        public Monitor(Guid id, string name, Uri endpoint)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNull(endpoint);
            
            Id = id;
            Name = name;
            Endpoint = endpoint;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Endpoint { get; set; }
        public int Interval { get; set; } // in seconds
        public bool IsActive { get; set; }
    }
}