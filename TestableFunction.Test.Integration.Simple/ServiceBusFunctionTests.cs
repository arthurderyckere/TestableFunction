using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TestableFunction.EventTriggers;
using TestableFunction.Test.Integration.Simple.Fakes;
using TestableFunction.Test.Integration.Simple.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TestableFunction.Test.Integration.Simple;

public class ServiceBusFunctionTests
{
    private FakeTableClient _fakeTableClient;
    private MyServiceBusFunction _sut;
    private XunitLogger<MyServiceBusFunction> _logger;

    // this is copied from timer function tests, a common fixture would now be helpful
    public ServiceBusFunctionTests(ITestOutputHelper testOutputHelper)
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

        _sut = new MyServiceBusFunction();
        _logger = new XunitLogger<MyServiceBusFunction>(testOutputHelper);
    }

    [Fact]
    public async Task CanRunServiceBusTrigger()
    {
        // WIP
        await _sut.RunAsync(FakeServiceBusReceivedMessage.GetQueueItems(), new FakeServiceBusMessageActions(), _logger);
    }
}

public class FakeServiceBusMessageActions : ServiceBusMessageActions
{

}

public class FakeServiceBusReceivedMessage
{
    public static ServiceBusReceivedMessage[] GetQueueItems()
    {
        var items = new[]
        {
            new
            {
                Data = BinaryData.FromString("message"),
                MessageId = "123456",
            },
            new
            {
                Data = BinaryData.FromString("message_2"),
                MessageId = "654321",
            }
        };

        return JsonConvert.DeserializeObject<ServiceBusReceivedMessage[]>(JsonConvert.SerializeObject(items));
    }

}
