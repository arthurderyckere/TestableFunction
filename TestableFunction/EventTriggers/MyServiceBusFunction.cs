using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TestableFunction.EventTriggers
{
    public class MyServiceBusFunction
    {
        [FunctionName("MyServiceBusFunction")]
        public async Task RunAsync([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage[] queueItems,
            ServiceBusMessageActions messageActions, ILogger log)
        {
            foreach (var item in queueItems)
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {item.MessageId}");
            }

            await Task.CompletedTask;
        }
    }
}
