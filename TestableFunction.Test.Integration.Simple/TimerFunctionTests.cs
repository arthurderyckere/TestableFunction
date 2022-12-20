using Azure.Data.Tables;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using System.Threading.Tasks;
using TestableFunction.Test.Integration.Simple.Fakes;
using TestableFunction.Test.Integration.Simple.Helpers;
using TestableFunction.TimerTriggers;
using Xunit;
using Xunit.Abstractions;

namespace TestableFunction.Test.Integration.Simple;

public class TimerFunctionTests : IClassFixture<FunctionFixture>
{
    private readonly XunitLogger<TimerFunctionTests> _logger;
    private readonly FakeTableClient _fakeTableClient;
    private readonly MyTimerFunction _sut;
    
    public TimerFunctionTests(FunctionFixture functionFixture, ITestOutputHelper testOutputHelper)
    {
        _logger = functionFixture.Logger<TimerFunctionTests>(testOutputHelper);
        _fakeTableClient = functionFixture.FakeTableClient;
        _sut = new MyTimerFunction((TableClient)functionFixture.Host.Services.GetService(typeof(TableClient)));
    }

    [Fact]
    public async Task CanTriggerTimerFunction()
    {
        await _sut.Run(new TimerInfo(new WeeklySchedule(), new ScheduleStatus()), _logger);
    }

    [Fact]
    public async Task TimerTriggerShouldAddEntryInTable()
    {
        await _sut.Run(new TimerInfo(new WeeklySchedule(), new ScheduleStatus()), _logger);

        _fakeTableClient.Entities.Should().HaveCount(1);
    }
}