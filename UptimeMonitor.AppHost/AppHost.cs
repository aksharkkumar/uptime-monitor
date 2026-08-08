var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var database = postgres.AddDatabase("UptimeMonitor");

var api = builder.AddProject<Projects.UptimeMonitor_Api>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpHealthCheck("/health");

// "WithReference" is used to ensure that the API project is built before the other projects
// "WaitFor" ensures that the API is running before the other projects start.
builder.AddProject<Projects.UptimeMonitor_Worker>("worker")
    .WithReference(api)
    .WithReference(database)
    .WaitFor(api)
    .WaitFor(database);

builder.AddProject<Projects.UptimeMonitor_Web>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
