using Xunit;

namespace Inertia.Core.Tests.Unit;

public class InertiaExtensionsTests
{
    [Fact]
    public void Lazy_Creates_LazyProp()
    {
        var lazy = InertiaExtensions.Lazy(() => "value");

        Assert.IsType<LazyProp>(lazy);
        Assert.Equal("value", ((LazyProp)lazy!).Value());
    }

    [Fact]
    public void Deferred_Creates_DeferredProp()
    {
        var deferred = InertiaExtensions.Deferred("group", () => "value");

        Assert.IsType<DeferredProp>(deferred);
        var prop = (DeferredProp)deferred!;
        Assert.Equal("group", prop.Group);
        Assert.Equal("value", prop.Value());
    }
}
