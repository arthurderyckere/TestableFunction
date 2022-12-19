using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TestableFunction;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TestableFunction;

public class Startup : FunctionsStartup
{
    // if you need to read config from json file: https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#customizing-configuration-sources        
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        ConfigureTableStorage(builder.Services, configuration);
    }

    private static void ConfigureTableStorage(IServiceCollection services, IConfiguration configuration)
    {
        var storageAccountFqdn = configuration["StorageOptions:Fqdn"];
        services.TryAddSingleton(_ => new TableClient(new Uri(storageAccountFqdn), "Logging", new DefaultAzureCredential()));
    }
}