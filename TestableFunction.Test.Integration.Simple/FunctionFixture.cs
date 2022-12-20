using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TestableFunction.Test.Integration.Simple.Fakes;
using TestableFunction.Test.Integration.Simple.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TestableFunction.Test.Integration.Simple;

public class FunctionFixture : IAsyncLifetime
{
    public FakeTableClient FakeTableClient { get; private set; } = null!;
    public XunitLogger<T> Logger<T>(ITestOutputHelper testOutputHelper) => new(testOutputHelper);

    public IHost Host { get; private set; } = null!;

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        FakeTableClient = new FakeTableClient();
        var startup = new Startup();
        Host = new HostBuilder()
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("local.settings.json");
            })
            .ConfigureWebJobs((context, builder) =>
            {
                builder.Services.AddSingleton<TableClient>(FakeTableClient);

                new Startup().Configure(new WebJobsBuilderContext
                {
                    ApplicationRootPath = context.HostingEnvironment.ContentRootPath,
                    Configuration = context.Configuration,
                    EnvironmentName = context.HostingEnvironment.EnvironmentName,
                }, builder);
            })
            .Build();

        return Task.CompletedTask;
    }
}
