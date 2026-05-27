using Microsoft.AspNetCore.Builder;

namespace Inertia.Core;

public static class InertiaMiddlewareExtensions
{
    public static IApplicationBuilder UseInertia(this IApplicationBuilder app)
    {
        return app.UseMiddleware<InertiaMiddleware>();
    }
}
