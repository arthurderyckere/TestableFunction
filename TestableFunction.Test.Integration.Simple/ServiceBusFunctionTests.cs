using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestableFunction.EventTriggers;
using TestableFunction.Test.Integration.Simple.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TestableFunction.Test.Integration.Simple;

public class ServiceBusFunctionTests : IClassFixture<FunctionFixture>
{
    private readonly XunitLogger<ServiceBusFunctionTests> _logger;
    private MyServiceBusFunction _sut;

    public ServiceBusFunctionTests(FunctionFixture functionFixture, ITestOutputHelper testOutputHelper)
    {
        _logger = functionFixture.Logger<ServiceBusFunctionTests>(testOutputHelper);
        _sut = new MyServiceBusFunction();   
    }

    [Fact]
    public async Task CanRunServiceBusTrigger()
    {
        // WIP
        await _sut.RunAsync(FakeServiceBusReceivedMessage.GetQueueItems().ToArray(), new FakeServiceBusMessageActions(), _logger);
    }
}

public class FakeServiceBusMessageActions : ServiceBusMessageActions
{

}

public class FakeServiceBusReceivedMessage
{
    public static IEnumerable<ServiceBusReceivedMessage> GetQueueItems()
    {
        yield return ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromString("message_1"), "message_1");
        yield return ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromString("message_2"), "message_2");
    }
}