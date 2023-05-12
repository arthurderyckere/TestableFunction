using FluentAssertions;
using Xunit.Abstractions;

namespace TestableFunction.Test.Collection.Fixture;

public class TestClass : TestClassBase
{
    public TestClass(ITestOutputHelper outputHelper, TestCollectionFixture fixture) : base(outputHelper, fixture)
    {
    }

    [Fact]
    public void FixtureShouldHaveBeenInitialized()
    {
        Fixture.IsInitialized.Should().BeTrue();
    }

    [Fact]
    public void FixtureShouldLog()
    {
        var action = () => Fixture.Log();

        var exception = Record.Exception(() => action());

        exception.Should().BeNull();
    }

    [Fact]
    public void FixtureOutputHelperShouldNotBeNull()
    {
        Fixture.OutputHelper.Should().NotBeNull();
    }

    [Fact]
    public void DependsOnOutputHelperShouldNotThrow()
    {
        var action = () => Fixture.DependsOnOutputHelper;

        var exception = Record.Exception(() => action());

        exception.Should().BeNull();
    }
}