using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Inertia.Core;

public class InertiaMiddleware
{
    private readonly RequestDelegate _next;

    public InertiaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (InertiaConstants.IsInertiaRequest(context))
        {
            context.Request.Headers.Accept = "application/json";
        }

        try
        {
            await _next(context);
        }
        catch (InertiaValidationException ex)
        {
            if (InertiaConstants.IsInertiaRequest(context))
            {
                await HandleValidationErrorAsync(context, ex);
                return;
            }
            throw;
        }

        if (InertiaConstants.IsInertiaRequest(context))
        {
            if (context.Response.StatusCode >= 300 && context.Response.StatusCode < 400)
            {
                var location = context.Response.Headers.Location.ToString();
                if (!string.IsNullOrEmpty(location))
                {
                    context.Response.StatusCode = 302;
                    context.Response.Headers[InertiaConstants.HeaderInertiaLocation] = location;
                    context.Response.Headers.Remove("Location");
                }
            }
        }
    }

    private static async Task HandleValidationErrorAsync(HttpContext context, InertiaValidationException ex)
    {
        context.Response.StatusCode = 422;
        context.Response.ContentType = "application/json";

        var response = new Dictionary<string, object?>
        {
            ["message"] = "The given data was invalid.",
            ["errors"] = ex.Errors
        };

        if (!string.IsNullOrEmpty(ex.ErrorBag))
        {
            context.Response.Headers[InertiaConstants.HeaderInertiaErrorBag] = ex.ErrorBag;
        }

        await context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
