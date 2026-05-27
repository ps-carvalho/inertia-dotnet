using Inertia.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Inertia.React.Tests.Unit;

public class ReactSSRRendererTests
{
    [Fact]
    public async Task RenderAsync_Returns_Null_When_No_Endpoint()
    {
        var handler = new TestHttpMessageHandler();
        var client = new HttpClient(handler);
        var options = new InertiaOptions();
        var renderer = new ReactSSRRenderer(client, options);

        var page = new Page { Component = "Home/Index" };
        var result = await renderer.RenderAsync(page);

        Assert.Null(result);
    }

    [Fact]
    public async Task RenderAsync_Returns_Body_From_SSR_Endpoint()
    {
        var response = new SSRResponse { Body = "<div>SSR</div>", Head = "<title>Test</title>" };
        var handler = new TestHttpMessageHandler(JsonSerializer.Serialize(response));
        var client = new HttpClient(handler);
        var options = new InertiaOptions { SSREndpoint = "http://localhost:13714/render" };
        var renderer = new ReactSSRRenderer(client, options);

        var page = new Page { Component = "Home/Index" };
        var result = await renderer.RenderAsync(page);

        Assert.Equal("<div>SSR</div>", result);
    }

    [Fact]
    public async Task RenderAsync_Returns_Null_On_Error()
    {
        var handler = new TestHttpMessageHandler(statusCode: HttpStatusCode.InternalServerError);
        var client = new HttpClient(handler);
        var options = new InertiaOptions { SSREndpoint = "http://localhost:13714/render" };
        var renderer = new ReactSSRRenderer(client, options);

        var page = new Page { Component = "Home/Index" };
        var result = await renderer.RenderAsync(page);

        Assert.Null(result);
    }
}

public class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly string? _response;
    private readonly HttpStatusCode _statusCode;

    public TestHttpMessageHandler(string? response = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _response = response;
        _statusCode = statusCode;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(_statusCode);
        if (_response != null)
        {
            response.Content = new StringContent(_response, Encoding.UTF8, "application/json");
        }
        return await Task.FromResult(response);
    }
}
