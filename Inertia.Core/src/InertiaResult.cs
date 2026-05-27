using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Inertia.Core;

public class InertiaResult : IActionResult
{
    private readonly string _component;
    private readonly Dictionary<string, object?> _props;

    public InertiaResult(string component, Dictionary<string, object?>? props = null)
    {
        _component = component;
        _props = props ?? new Dictionary<string, object?>();
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var httpContext = context.HttpContext;
        var inertia = httpContext.RequestServices.GetRequiredService<IInertia>();
        
        var url = httpContext.Request.Path + httpContext.Request.QueryString;
        var version = inertia.GetVersion();

        if (!InertiaConstants.IsInertiaVersionMatch(httpContext, version))
        {
            httpContext.Response.StatusCode = 409;
            httpContext.Response.Headers[InertiaConstants.HeaderInertiaLocation] = url;
            return;
        }

        var props = await ResolvePropsAsync(httpContext, inertia);

        var page = new Page
        {
            Component = _component,
            Props = props,
            Url = url,
            Version = version
        };

        if (InertiaConstants.IsInertiaRequest(httpContext))
        {
            await WriteJsonResponse(httpContext, page);
        }
        else
        {
            await RenderRootViewAsync(context, page);
        }
    }

    private async Task RenderRootViewAsync(ActionContext context, Page page)
    {
        var httpContext = context.HttpContext;
        var options = httpContext.RequestServices.GetRequiredService<InertiaOptions>();
        var ssrRenderer = httpContext.RequestServices.GetService<ISSRRenderer>();

        string? ssrHtml = null;
        if (options.EnableSSR && ssrRenderer != null)
        {
            ssrHtml = await ssrRenderer.RenderAsync(page);
        }

        var pageJson = JsonSerializer.Serialize(page, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var viewEngine = httpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
        var tempDataProvider = httpContext.RequestServices.GetRequiredService<ITempDataProvider>();
        var viewName = options.RootView;

        var viewResult = viewEngine.FindView(context, viewName, isMainPage: true);
        if (!viewResult.Success)
        {
            viewResult = viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
        }

        if (!viewResult.Success)
        {
            // Fallback: write JSON if view not found
            await WriteJsonResponse(httpContext, page);
            return;
        }

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
        {
            ["page"] = page,
            ["pageJson"] = pageJson,
            ["ssrHtml"] = ssrHtml
        };

        var tempData = new TempDataDictionary(httpContext, tempDataProvider);
        await using var writer = new StreamWriter(httpContext.Response.Body, leaveOpen: true);
        var viewContext = new ViewContext(context, viewResult.View, viewData, tempData, writer, new HtmlHelperOptions());

        httpContext.Response.ContentType = "text/html; charset=utf-8";
        await viewResult.View.RenderAsync(viewContext);
        await writer.FlushAsync();
    }

    private static async Task WriteJsonResponse(HttpContext httpContext, Page page)
    {
        httpContext.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(page, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        await httpContext.Response.WriteAsync(json);
    }

    private async Task<Dictionary<string, object?>> ResolvePropsAsync(HttpContext context, IInertia inertia)
    {
        var resolvedProps = new Dictionary<string, object?>();
        var sharedData = inertia.GetSharedData();

        foreach (var item in sharedData)
        {
            resolvedProps[item.Key] = await ResolveValueAsync(item.Value);
        }

        var partialData = InertiaConstants.GetPartialData(context);
        var partialExcept = InertiaConstants.GetPartialExcept(context);

        foreach (var item in _props)
        {
            if (partialData != null && !partialData.Contains(item.Key))
                continue;

            if (partialExcept != null && partialExcept.Contains(item.Key))
                continue;

            resolvedProps[item.Key] = await ResolveValueAsync(item.Value);
        }

        return resolvedProps;
    }

    private static async Task<object?> ResolveValueAsync(object? value)
    {
        if (value is LazyProp lazy)
            return lazy.Value();

        if (value is DeferredProp deferred)
            return deferred.Value();

        if (value is Func<Task<object?>> asyncFunc)
            return await asyncFunc();

        if (value is Func<object?> func)
            return func();

        return value;
    }
}
