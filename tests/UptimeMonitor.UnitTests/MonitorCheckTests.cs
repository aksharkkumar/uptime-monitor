using UptimeMonitor.Domain.MonitorChecks;

namespace UptimeMonitor.UnitTests;
public sealed class MonitorCheckTests
{
    [Fact]
    public void MonitorCheckPreservesTheCurrentMonitorCheckState()
    {
        var checkId = Guid.NewGuid();
        var monitorId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var responseTimeMs = 150;
        var statusCode = 200;
        var expectedStatusCode = 200;

        var monitorCheck = new MonitorCheck(
            checkId,
            monitorId,
            timestamp,
            responseTimeMs,
            statusCode,
            expectedStatusCode);
        
        Assert.Equal(checkId, monitorCheck.Id);
        Assert.Equal(monitorId, monitorCheck.MonitorId); 
        Assert.Equal(timestamp, monitorCheck.CheckedAt);
        Assert.Equal(responseTimeMs, monitorCheck.LatencyMs);
        Assert.Equal(statusCode, monitorCheck.StatusCode);
        Assert.Equal(expectedStatusCode, monitorCheck.ExpectedStatusCode);
    }

    [Theory]
    [InlineData(200, 200, true)]
    [InlineData(204, 200, false)]
    [InlineData(500, 500, true)]
    public void IsSuccessIsDerivedFromActualAndExpectedStatusCodes(
        int statusCode,
        int expectedStatusCode,
        bool expectedIsSuccess)
    {
        var monitorCheck = CreateMonitorCheck(
            statusCode: statusCode,
            expectedStatusCode: expectedStatusCode);

        Assert.Equal(expectedIsSuccess, monitorCheck.IsSuccess);
    }

    [Fact]
    public void MonitorCheckRejectsEmptyCheckId()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => CreateMonitorCheck(id: Guid.Empty));

        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void MonitorCheckRejectsEmptyMonitorId()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => CreateMonitorCheck(monitorId: Guid.Empty));

        Assert.Equal("monitorId", exception.ParamName);
    }

    [Fact]
    public void MonitorCheckRejectsNegativeLatency()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CreateMonitorCheck(responseTimeMs: -1));

        Assert.Equal("latencyMs", exception.ParamName);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(600)]
    public void MonitorCheckRejectsInvalidStatusCode(int statusCode)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CreateMonitorCheck(statusCode: statusCode));

        Assert.Equal("statusCode", exception.ParamName);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(600)]
    public void MonitorCheckRejectsInvalidExpectedStatusCode(int expectedStatusCode)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CreateMonitorCheck(expectedStatusCode: expectedStatusCode));

        Assert.Equal("expectedStatusCode", exception.ParamName);
    }

    private static MonitorCheck CreateMonitorCheck(
        Guid? id = null,
        Guid? monitorId = null,
        int responseTimeMs = 150,
        int statusCode = 200,
        int expectedStatusCode = 200)
    {
        return new MonitorCheck(
            id ?? Guid.NewGuid(),
            monitorId ?? Guid.NewGuid(),
            DateTime.UtcNow,
            responseTimeMs,
            statusCode,
            expectedStatusCode);
    }
}
