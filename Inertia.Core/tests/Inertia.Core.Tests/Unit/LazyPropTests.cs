using Xunit;

namespace Inertia.Core.Tests.Unit;

public class LazyPropTests
{
    [Fact]
    public void LazyProp_Stores_Function()
    {
        var lazy = new LazyProp(() => "resolved");

        Assert.NotNull(lazy.Value);
        Assert.Equal("resolved", lazy.Value());
    }

    [Fact]
    public void LazyProp_Resolves_Each_Time()
    {
        var counter = 0;
        var lazy = new LazyProp(() => ++counter);

        Assert.Equal(1, lazy.Value());
        Assert.Equal(2, lazy.Value());
    }
}

public class DeferredPropTests
{
    [Fact]
    public void DeferredProp_Stores_Group_And_Function()
    {
        var deferred = new DeferredProp("stats", () => "data");

        Assert.Equal("stats", deferred.Group);
        Assert.NotNull(deferred.Value);
        Assert.Equal("data", deferred.Value());
    }
}
