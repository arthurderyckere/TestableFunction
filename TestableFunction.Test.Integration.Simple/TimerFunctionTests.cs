using Azure.Data.Tables;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TestableFunction.Test.Integration.Simple.Fakes;
using TestableFunction.Test.Integration.Simple.Helpers;
using TestableFunction.TimerTriggers;
using Xunit;
using Xunit.Abstractions;

namespace TestableFunction.Test.Integration.Simple;

public class TimerFunctionTests
{
    private readonly MyTimerFunction _sut;
    private XunitLogger<MyTimerFunction> _logger;
    private FakeTableClient _fakeTableClient;

    // in XUnit, the constructor will run before each test, resulting in a fresh environment per test.
    // could use a test collection a common fixture to speed up the test and or share the host
    public TimerFunctionTests(ITestOutputHelper testOutputHelper)
    {
        _fakeTableClient = new FakeTableClient();

        var startup = new Startup();
        var host = new HostBuilder()
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("local.settings.json");
            })
            .ConfigureWebJobs((context, builder) =>
            {
                builder.Services.AddSingleton<TableClient>(_fakeTableClient);

                new Startup().Configure(new WebJobsBuilderContext
                {
                    ApplicationRootPath = context.HostingEnvironment.ContentRootPath,
                    Configuration = context.Configuration,
                    EnvironmentName = context.HostingEnvironment.EnvironmentName,
                }, builder);
            })
            .Build();

        _sut = new MyTimerFunction((TableClient)host.Services.GetService(typeof(TableClient)));
        _logger = new XunitLogger<MyTimerFunction>(testOutputHelper);
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