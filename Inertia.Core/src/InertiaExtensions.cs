using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Inertia.Core;

public static class InertiaExtensions
{
    public static IServiceCollection AddInertia(this IServiceCollection services, Action<InertiaOptions>? configureOptions = null)
    {
        var options = new InertiaOptions();
        configureOptions?.Invoke(options);
        
        services.AddSingleton(options);
        services.AddHttpContextAccessor();
        services.AddScoped<IInertia, InertiaService>();
        services.AddSingleton<ISSRRenderer, DefaultSSRRenderer>();
        
        return services;
    }

    public static IServiceCollection AddInertiaSSR<TRenderer>(this IServiceCollection services) where TRenderer : class, ISSRRenderer
    {
        services.AddSingleton<ISSRRenderer, TRenderer>();
        return services;
    }

    public static InertiaResult Inertia(this ControllerBase controller, string component, Dictionary<string, object?>? props = null)
    {
        return new InertiaResult(component, props);
    }

    public static InertiaResult Inertia(this ControllerBase controller, string component, object props)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var prop in props.GetType().GetProperties())
        {
            dict[prop.Name] = prop.GetValue(props);
        }
        return new InertiaResult(component, dict);
    }

    public static object? Lazy(Func<object?> value)
    {
        return new LazyProp(value);
    }

    public static object? Deferred(string group, Func<object?> value)
    {
        return new DeferredProp(group, value);
    }

    public static void InertiaValidate(this ControllerBase controller, ModelStateDictionary modelState, string? errorBag = null)
    {
        if (!modelState.IsValid)
        {
            var inertia = controller.HttpContext.RequestServices.GetRequiredService<IInertia>();
            inertia.SetErrorBag(errorBag);
            throw new InertiaValidationException(modelState, errorBag);
        }
    }
}
