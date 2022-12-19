using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TestableFunction.Models;

namespace TestableFunction.TimerTriggers;

public class MyTimerFunction
{
    private readonly TableClient _client;

    public MyTimerFunction(TableClient client)
    {
        _client = client;
    }

    [FunctionName("MyTimerFunction")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
    {
        var message = $"C# Timer trigger function executed at: {DateTime.Now}";
        log.LogInformation(message);

        await _client.AddEntityAsync(LogEntity.Create(message));
    }
}
