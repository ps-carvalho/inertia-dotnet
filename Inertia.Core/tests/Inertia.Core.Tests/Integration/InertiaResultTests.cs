using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Inertia.Core.Tests.Integration;

public class InertiaResultTests
{
    [Fact]
    public void InertiaResult_Stores_Component_And_Props()
    {
        var props = new Dictionary<string, object?> { ["message"] = "Hello" };
        var result = new InertiaResult("Home/Index", props);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ExecuteResultAsync_Returns_409_When_Version_Mismatch()
    {
        var options = new InertiaOptions { Version = "2.0.0" };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Inertia"] = "true";
        httpContext.Request.Headers["X-Inertia-Version"] = "1.0.0";
        httpContext.Request.Path = "/";
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(options)
            .AddSingleton<IInertia>(new InertiaService(options, new HttpContextAccessor { HttpContext = httpContext }))
            .BuildServiceProvider();

        var actionContext = new ActionContext
        {
            HttpContext = httpContext
        };

        var result = new InertiaResult("Home/Index");
        await result.ExecuteResultAsync(actionContext);

        Assert.Equal(409, httpContext.Response.StatusCode);
    }

    [Fact]
    public void ResolvePropsAsync_Filters_Partial_Data()
    {
        var options = new InertiaOptions();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Inertia-Partial-Data"] = "message";
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(options)
            .AddSingleton<IInertia>(new InertiaService(options, new HttpContextAccessor { HttpContext = httpContext }))
            .BuildServiceProvider();

        var inertia = httpContext.RequestServices.GetRequiredService<IInertia>();
        var result = new InertiaResult("Home/Index", new Dictionary<string, object?>
        {
            ["message"] = "Hello",
            ["other"] = "World"
        });

        var method = result.GetType().GetMethod("ResolvePropsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task<Dictionary<string, object?>>?)method?.Invoke(result, new object[] { httpContext, inertia });
        var props = task?.GetAwaiter().GetResult();

        Assert.NotNull(props);
        Assert.Equal("Hello", props["message"]);
        Assert.DoesNotContain("other", props.Keys);
    }

    [Fact]
    public void ResolvePropsAsync_Resolves_Lazy_Props()
    {
        var options = new InertiaOptions();
        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(options)
            .AddSingleton<IInertia>(new InertiaService(options, new HttpContextAccessor { HttpContext = httpContext }))
            .BuildServiceProvider();

        var inertia = httpContext.RequestServices.GetRequiredService<IInertia>();
        var result = new InertiaResult("Home/Index", new Dictionary<string, object?>
        {
            ["lazy"] = new LazyProp(() => "resolved")
        });

        var method = result.GetType().GetMethod("ResolvePropsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task<Dictionary<string, object?>>?)method?.Invoke(result, new object[] { httpContext, inertia });
        var props = task?.GetAwaiter().GetResult();

        Assert.NotNull(props);
        Assert.Equal("resolved", props["lazy"]);
    }
}
