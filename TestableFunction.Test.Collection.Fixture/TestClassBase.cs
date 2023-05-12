using Xunit.Abstractions;

namespace TestableFunction.Test.Collection.Fixture;

[Collection("Test Collection")]
public class TestClassBase
{
    public TestClassBase(ITestOutputHelper outputHelper, TestCollectionFixture fixture)
    {
        Fixture = fixture;
        Fixture.SetOutputHelper(outputHelper);
    }

    public TestCollectionFixture Fixture { get; }
}
