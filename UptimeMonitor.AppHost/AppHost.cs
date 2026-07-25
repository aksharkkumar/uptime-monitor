var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.UptimeMonitor_Api>("api")
    .WithHttpHealthCheck("/health");

// "WithReference" is used to ensure that the API project is built before the other projects
// "WaitFor" ensures that the API is running before the other projects start.
builder.AddProject<Projects.UptimeMonitor_Worker>("worker")
    .WithReference(api)
    .WaitFor(api);

builder.AddProject<Projects.UptimeMonitor_Web>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
