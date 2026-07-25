# Uptime Monitor

An uptime monitoring platform built with .NET 10 and Aspire 13.4.

## Epic 1: project foundation

The initial solution provides:

- an Aspire AppHost that orchestrates the API and monitoring worker;
- shared service defaults for OpenTelemetry, health checks, service discovery, and resilient HTTP clients;
- a minimal HTTP API with readiness, liveness, OpenAPI, and service-status endpoints;
- a background worker ready to host scheduled endpoint probes;
- repository-wide nullable analysis and warnings-as-errors.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- an OCI-compatible container runtime if future Aspire resources require containers

## Run locally

```bash
dotnet run --project UptimeMonitor.AppHost
```

The command prints the Aspire dashboard URL. Use the dashboard to open the API, inspect logs, traces, metrics, and health state.

## Verify

```bash
dotnet build UptimeMonitor.sln
dotnet test UptimeMonitor.sln --no-build
```

## Next epic

Add persisted monitor definitions and scheduling, then replace the worker heartbeat with HTTP probes. The AppHost is ready for database and messaging integrations when those requirements are defined.
