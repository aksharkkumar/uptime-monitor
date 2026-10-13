using UptimeMonitor.Api;
using UptimeMonitor.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(allowIntegerValues: false)
    );
});

builder.AddServiceDefaults();
builder.AddInfrastructure();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => Results.Redirect("/status"));
app.MapGet("/status", () => new ServiceStatus(
        Service: "uptime-monitor-api",
        Status: "ready",
        Timestamp: DateTimeOffset.UtcNow))
    .WithName("GetServiceStatus")
    .WithTags("System");

app.MapDefaultEndpoints();
app.Run();
