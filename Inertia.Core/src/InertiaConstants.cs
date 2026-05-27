using Microsoft.AspNetCore.Http;

namespace Inertia.Core;

public static class InertiaConstants
{
    public const string HeaderInertia = "X-Inertia";
    public const string HeaderInertiaVersion = "X-Inertia-Version";
    public const string HeaderInertiaLocation = "X-Inertia-Location";
    public const string HeaderInertiaPartialData = "X-Inertia-Partial-Data";
    public const string HeaderInertiaPartialExcept = "X-Inertia-Partial-Except";
    public const string HeaderInertiaErrorBag = "X-Inertia-Error-Bag";

    public static bool IsInertiaRequest(HttpContext context)
    {
        return context.Request.Headers.ContainsKey(HeaderInertia);
    }

    public static bool IsInertiaVersionMatch(HttpContext context, string? version)
    {
        var requestVersion = context.Request.Headers[HeaderInertiaVersion].ToString();
        return string.IsNullOrEmpty(requestVersion) || requestVersion == version;
    }

    public static string[]? GetPartialData(HttpContext context)
    {
        var partialData = context.Request.Headers[HeaderInertiaPartialData].ToString();
        return string.IsNullOrEmpty(partialData) ? null : partialData.Split(',');
    }

    public static string[]? GetPartialExcept(HttpContext context)
    {
        var partialExcept = context.Request.Headers[HeaderInertiaPartialExcept].ToString();
        return string.IsNullOrEmpty(partialExcept) ? null : partialExcept.Split(',');
    }
}
