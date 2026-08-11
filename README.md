# Uptime Monitor

Uptime Monitor is a .NET 10 application for tracking endpoint availability and
performance. The solution uses .NET Aspire to orchestrate a Blazor web app, an
HTTP API, a background worker, and PostgreSQL.

## Current status

The project is under active development. It currently includes:

- domain models for endpoint monitors, checks, and monitor events;
- Entity Framework Core persistence backed by PostgreSQL;
- configurable alert thresholds and cooldowns;
- an API with service-status, readiness, liveness, and development OpenAPI
  endpoints;
- a background worker with a periodic heartbeat, ready for the endpoint-check
  scheduling workflow;
- a Blazor web project with the initial application shell;
- shared service discovery, HTTP resilience, health checks, logging, metrics,
  and distributed tracing through Aspire service defaults; and
- unit tests for the domain, configuration, persistence, and API status model.

Endpoint probing, alert delivery, and the user-facing monitoring experience are
not implemented yet.

## Architecture

| Project | Responsibility |
| --- | --- |
| `UptimeMonitor.AppHost` | Orchestrates the application and PostgreSQL with .NET Aspire. |
| `UptimeMonitor.ServiceDefaults` | Provides shared observability, health checks, service discovery, and HTTP resilience. |
| `src/UptimeMonitor.Web` | Hosts the Blazor web application. |
| `src/UptimeMonitor.Api` | Exposes the HTTP API and composes infrastructure services. |
| `src/UptimeMonitor.Worker` | Hosts background monitoring work. |
| `src/UptimeMonitor.Domain` | Contains monitor, check, and event domain models. |
| `src/UptimeMonitor.Infrastructure` | Configures PostgreSQL, Entity Framework Core, and application settings. |
| `tests/UptimeMonitor.UnitTests` | Contains the xUnit test suite. |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (the solution
  is pinned to SDK `10.0.302` and allows newer 10.0 feature bands)
- an OCI-compatible container runtime, such as Docker Desktop or Podman, for
  the PostgreSQL resource

## Run locally

Restore dependencies and start the Aspire AppHost:

```bash
dotnet restore UptimeMonitor.sln
dotnet run --project UptimeMonitor.AppHost
```

The command output includes the Aspire dashboard URL. From the dashboard you
can open the web app and API, and inspect resource health, logs, traces, and
metrics. PostgreSQL data is stored in an Aspire-managed container volume and is
preserved across restarts.

In the Development environment, the API exposes:

- `/status` for basic service status;
- `/health` for readiness;
- `/alive` for liveness; and
- `/openapi/v1.json` for the OpenAPI document.

## Configuration

The API and worker read alert behavior from the `AlertSettings` configuration
section:

```json
{
  "AlertSettings": {
    "Enabled": true,
    "FailureThreshold": 3,
    "CooldownSeconds": 300
  }
}
```

Aspire supplies the `UptimeMonitor` PostgreSQL connection string to the API and
worker when they are launched through the AppHost. Use standard .NET
configuration providers or user secrets for local overrides; do not commit
credentials.

## Build and test

```bash
dotnet build UptimeMonitor.sln
dotnet test UptimeMonitor.sln --no-build
```

Nullable reference types, recommended analyzers, and warnings-as-errors are
enabled across the repository.
