using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using UptimeMonitor.Domain;
using UptimeMonitor.Domain.MonitorChecks;
using UptimeMonitor.Domain.MonitorEvents;
using UptimeMonitor.Domain.Monitors;
using UptimeMonitor.Infrastructure;
using UptimeMonitor.Infrastructure.Configuration;
using UptimeMonitor.Infrastructure.Persistence;

namespace UptimeMonitor.UnitTests;

public sealed class PersistenceTests
{
    [Fact]
    public void DbContextExposesRequiredEntitySets()
    {
        Assert.Equal(
            typeof(DbSet<EndpointMonitor>),
            typeof(UptimeMonitorDbContext).GetProperty(nameof(UptimeMonitorDbContext.Monitors))?.PropertyType);
        Assert.Equal(
            typeof(DbSet<MonitorCheck>),
            typeof(UptimeMonitorDbContext).GetProperty(nameof(UptimeMonitorDbContext.MonitorChecks))?.PropertyType);
        Assert.Equal(
            typeof(DbSet<MonitorEvent>),
            typeof(UptimeMonitorDbContext).GetProperty(nameof(UptimeMonitorDbContext.MonitorEvents))?.PropertyType);
    }

    [Fact]
    public void InfrastructureRegistersPersistenceAndConfigurationServices()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:UptimeMonitor"] = "Host=localhost;Database=uptime_monitor",
            ["AlertSettings:Enabled"] = "false",
            ["AlertSettings:FailureThreshold"] = "5",
            ["AlertSettings:CooldownSeconds"] = "120"
        });

        builder.AddInfrastructure();

        using var serviceProvider = builder.Services.BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<UptimeMonitorDbContext>();
        Assert.NotNull(context.Model);

        var healthChecks = serviceProvider
            .GetRequiredService<IOptions<HealthCheckServiceOptions>>()
            .Value
            .Registrations;
        Assert.NotEmpty(healthChecks);

        var settings = serviceProvider
            .GetRequiredService<IOptions<AlertSettings>>()
            .Value;
        Assert.False(settings.Enabled);
        Assert.Equal(5, settings.FailureThreshold);
        Assert.Equal(120, settings.CooldownSeconds);
    }

    [Fact]
    public void DbContextModelCanBeCreated()
    {
        using var context = CreateContext();

        Assert.NotNull(context.Model);
    }

    [Fact]
    public void EntitiesHaveExpectedScalarMappings()
    {
        using var context = CreateContext();

        AssertEntityMapping<EndpointMonitor>(context, "Monitors");
        AssertEntityMapping<MonitorCheck>(context, "MonitorChecks");
        AssertEntityMapping<MonitorEvent>(context, "MonitorEvents");

        var monitorType = context.Model.FindEntityType(typeof(EndpointMonitor));
        var endpoint = monitorType?.FindProperty(nameof(EndpointMonitor.Endpoint));
        Assert.Equal(typeof(string), endpoint?.GetTypeMapping().Converter?.ProviderClrType);

        var checkType = context.Model.FindEntityType(typeof(MonitorCheck));
        Assert.Null(checkType?.FindProperty(nameof(MonitorCheck.IsSuccess)));

        var eventType = context.Model.FindEntityType(typeof(MonitorEvent));
        Assert.Null(eventType?.FindProperty(nameof(MonitorEvent.IsResolved)));
    }

    [Fact]
    public void MonitorEventTypeUsesNativePostgresEnum()
    {
        using var context = CreateContext();

        var designTimeModel = context.GetService<IDesignTimeModel>().Model;
        var postgresEnum = Assert.Single(
            designTimeModel.GetPostgresEnums(),
            item => item.Name == "monitor_event_type");

        Assert.Equal(
            ["monitor_down", "monitor_recovered", "slow_response", "tls_certificate_expiring"],
            postgresEnum.Labels);
    }

    [Fact]
    public void EntitiesHaveExpectedRelationships()
    {
        using var context = CreateContext();

        AssertForeignKey<MonitorCheck, EndpointMonitor>(
            context,
            nameof(MonitorCheck.MonitorId),
            isRequired: true);
        AssertForeignKey<MonitorEvent, EndpointMonitor>(
            context,
            nameof(MonitorEvent.MonitorId),
            isRequired: true);
        AssertForeignKey<MonitorEvent, MonitorCheck>(
            context,
            nameof(MonitorEvent.TriggeringCheckId),
            isRequired: false);
    }

    private static UptimeMonitorDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<UptimeMonitorDbContext>()
            .UseNpgsql(
                "Host=localhost;Database=uptime_monitor",
                npgsqlOptions => npgsqlOptions.MapEnum<MonitorEventType>("monitor_event_type"))
            .Options;

        return new UptimeMonitorDbContext(options);
    }

    private static void AssertEntityMapping<TEntity>(
        UptimeMonitorDbContext context,
        string expectedTableName)
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));

        Assert.NotNull(entityType);
        Assert.Equal(expectedTableName, entityType.GetTableName());
        var keyProperty = Assert.Single(entityType.FindPrimaryKey()!.Properties);
        Assert.Equal("Id", keyProperty.Name);
        Assert.Equal(ValueGenerated.Never, keyProperty.ValueGenerated);
    }

    private static void AssertForeignKey<TDependent, TPrincipal>(
        UptimeMonitorDbContext context,
        string propertyName,
        bool isRequired)
    {
        var dependentType = context.Model.FindEntityType(typeof(TDependent));
        var foreignKey = Assert.Single(
            dependentType!.GetForeignKeys(),
            item => item.PrincipalEntityType.ClrType == typeof(TPrincipal)
                && item.Properties.Single().Name == propertyName);

        Assert.Equal(isRequired, foreignKey.IsRequired);
    }
}
