using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UptimeMonitor.Infrastructure;
using UptimeMonitor.Infrastructure.Configuration;

namespace UptimeMonitor.UnitTests;

public sealed class ConfigurationTests
{
    public static TheoryData<string> ApplicationSettings => new()
    {
        "Api",
        "Worker"
    };

    [Fact]
    public void AlertSettingsBindFromConfiguration()
    {
        var configuration = BuildInMemoryConfiguration(new Dictionary<string, string?>
        {
            ["AlertSettings:Enabled"] = "false",
            ["AlertSettings:FailureThreshold"] = "5",
            ["AlertSettings:CooldownSeconds"] = "120"
        });

        using var serviceProvider = BuildServiceProvider(configuration);

        var settings = serviceProvider
            .GetRequiredService<IOptions<AlertSettings>>()
            .Value;

        Assert.False(settings.Enabled);
        Assert.Equal(5, settings.FailureThreshold);
        Assert.Equal(120, settings.CooldownSeconds);
    }

    [Theory]
    [InlineData("AlertSettings:FailureThreshold", "0")]
    [InlineData("AlertSettings:CooldownSeconds", "-1")]
    public void InvalidAlertSettingsFailValidation(string key, string invalidValue)
    {
        var values = ValidAlertSettings();
        values[key] = invalidValue;
        var configuration = BuildInMemoryConfiguration(values);

        using var serviceProvider = BuildServiceProvider(configuration);
        var options = serviceProvider.GetRequiredService<IOptions<AlertSettings>>();

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Theory]
    [MemberData(nameof(ApplicationSettings))]
    public void DevelopmentSettingsOverrideBaseSettings(string application)
    {
        var configuration = BuildApplicationConfiguration(application);

        var settings = configuration
            .GetSection(AlertSettings.SectionName)
            .Get<AlertSettings>();

        Assert.NotNull(settings);
        Assert.True(settings.Enabled);
        Assert.Equal(1, settings.FailureThreshold);
        Assert.Equal(30, settings.CooldownSeconds);
    }

    [Fact]
    public void ConnectionStringCanBeRetrievedByExpectedName()
    {
        const string expected = "Host=localhost;Database=uptime_monitor";
        var configuration = BuildInMemoryConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:UptimeMonitor"] = expected
        });

        Assert.Equal(expected, configuration.GetConnectionString("UptimeMonitor"));
    }

    [Theory]
    [MemberData(nameof(ApplicationSettings))]
    public void CommittedSettingsDoNotContainConnectionStringSecrets(string application)
    {
        var configuration = BuildApplicationConfiguration(application);

        Assert.Equal(string.Empty, configuration.GetConnectionString("UptimeMonitor"));
    }

    private static IConfigurationRoot BuildApplicationConfiguration(string application)
    {
        var settingsPath = Path.Combine(
            AppContext.BaseDirectory,
            "TestSettings",
            application);

        return new ConfigurationBuilder()
            .SetBasePath(settingsPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();
    }

    private static IConfigurationRoot BuildInMemoryConfiguration(
        IEnumerable<KeyValuePair<string, string?>> values) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();

    private static Dictionary<string, string?> ValidAlertSettings() => new()
    {
        ["AlertSettings:Enabled"] = "true",
        ["AlertSettings:FailureThreshold"] = "3",
        ["AlertSettings:CooldownSeconds"] = "300"
    };

    private static ServiceProvider BuildServiceProvider(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);
        return services.BuildServiceProvider();
    }
}
