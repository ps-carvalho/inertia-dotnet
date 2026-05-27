using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Inertia.React.Tests.Integration;

public class ReactInertiaExtensionsTests
{
    [Fact]
    public void AddInertiaReact_Registers_Services()
    {
        var services = new ServiceCollection();
        services.AddInertiaReact(options =>
        {
            options.Version = "1.0.0";
        });

        var provider = services.BuildServiceProvider();
        var inertia = provider.GetService<Inertia.Core.IInertia>();

        Assert.NotNull(inertia);
    }

    [Fact]
    public void AddInertiaReact_Registers_SSR_Renderer()
    {
        var services = new ServiceCollection();
        services.AddInertiaReact(options =>
        {
            options.SSREndpoint = "http://localhost:13714/render";
        });

        var provider = services.BuildServiceProvider();
        var renderer = provider.GetService<Inertia.Core.ISSRRenderer>();

        Assert.NotNull(renderer);
        Assert.IsType<ReactSSRRenderer>(renderer);
    }

    [Fact]
    public void AddInertiaReact_Registers_HttpClient()
    {
        var services = new ServiceCollection();
        services.AddInertiaReact();

        var provider = services.BuildServiceProvider();
        var factory = provider.GetService<IHttpClientFactory>();

        Assert.NotNull(factory);
    }
}
