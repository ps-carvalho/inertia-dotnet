using Xunit;

namespace Inertia.Core.Tests.Unit;

public class InertiaOptionsTests
{
    [Fact]
    public void Default_RootView_Is_Set()
    {
        var options = new InertiaOptions();

        Assert.Equal("_Inertia", options.RootView);
    }

    [Fact]
    public void Default_Version_Is_Null()
    {
        var options = new InertiaOptions();

        Assert.Null(options.Version);
    }

    [Fact]
    public void Default_EnableSSR_Is_False()
    {
        var options = new InertiaOptions();

        Assert.False(options.EnableSSR);
    }

    [Fact]
    public void Can_Configure_Version()
    {
        var options = new InertiaOptions
        {
            Version = "1.2.3"
        };

        Assert.Equal("1.2.3", options.Version);
    }

    [Fact]
    public void Can_Configure_RootView()
    {
        var options = new InertiaOptions
        {
            RootView = "CustomLayout"
        };

        Assert.Equal("CustomLayout", options.RootView);
    }

    [Fact]
    public void Can_Configure_VersionResolver()
    {
        var options = new InertiaOptions
        {
            VersionResolver = _ => "resolved-version"
        };

        Assert.NotNull(options.VersionResolver);
        Assert.Equal("resolved-version", options.VersionResolver(null!));
    }
}
