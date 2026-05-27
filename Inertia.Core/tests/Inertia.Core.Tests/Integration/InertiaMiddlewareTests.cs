using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Inertia.Core.Tests.Integration;

public class InertiaMiddlewareTests
{
    [Fact]
    public async Task Middleware_Sets_Accept_Header_For_Inertia_Requests()
    {
        var middleware = new InertiaMiddleware(async (context) =>
        {
            Assert.Equal("application/json", context.Request.Headers.Accept.ToString());
            await Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Headers["X-Inertia"] = "true";

        await middleware.InvokeAsync(context);
    }

    [Fact]
    public async Task Middleware_Handles_Redirects_For_Inertia_Requests()
    {
        var middleware = new InertiaMiddleware(async (context) =>
        {
            context.Response.StatusCode = 302;
            context.Response.Headers.Location = "/other";
            await Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Headers["X-Inertia"] = "true";

        await middleware.InvokeAsync(context);

        Assert.Equal(302, context.Response.StatusCode);
        Assert.Equal("/other", context.Response.Headers["X-Inertia-Location"].ToString());
        Assert.False(context.Response.Headers.ContainsKey("Location"));
    }

    [Fact]
    public async Task Middleware_Handles_Validation_Exception()
    {
        var middleware = new InertiaMiddleware(async (context) =>
        {
            throw new InertiaValidationException(
                new Dictionary<string, string> { ["field"] = "Error" }, "createForm");
        });

        var context = new DefaultHttpContext();
        context.Request.Headers["X-Inertia"] = "true";

        await middleware.InvokeAsync(context);

        Assert.Equal(422, context.Response.StatusCode);
        Assert.Equal("createForm", context.Response.Headers["X-Inertia-Error-Bag"].ToString());
    }
}
