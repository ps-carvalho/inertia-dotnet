using Inertia.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Inertia.React;

public static class ReactInertiaExtensions
{
    public static IServiceCollection AddInertiaReact(this IServiceCollection services, Action<InertiaOptions>? configureOptions = null)
    {
        services.AddInertia(options =>
        {
            configureOptions?.Invoke(options);
        });

        services.AddHttpClient<ReactSSRRenderer>((sp, client) =>
        {
            var options = sp.GetRequiredService<InertiaOptions>();
            if (!string.IsNullOrEmpty(options.SSREndpoint))
            {
                client.BaseAddress = new Uri(options.SSREndpoint);
            }
        });

        services.AddScoped<ISSRRenderer>(sp =>
        {
            var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(ReactSSRRenderer).Name);
            var options = sp.GetRequiredService<InertiaOptions>();
            return new ReactSSRRenderer(httpClient, options);
        });

        return services;
    }
}
