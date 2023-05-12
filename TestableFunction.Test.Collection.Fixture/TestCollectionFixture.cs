using Xunit.Abstractions;

namespace TestableFunction.Test.Collection.Fixture;

public class TestCollectionFixture : IAsyncLifetime
{
    public ITestOutputHelper? OutputHelper { get; private set; }
    public bool? IsInitialized { get; set; }
    public Action? DependsOnOutputHelper { get; private set; }

    public TestCollectionFixture()
    {

    }

    public void SetOutputHelper(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        await LongRunningFunction();
    }

    public async Task LongRunningFunction()
    {
        await Task.Delay(1000);
        IsInitialized = true;
        DependsOnOutputHelper = Log;
    }

    public void Log()
    {
        OutputHelper!.WriteLine("This proves that output helper works.");
    }
}
