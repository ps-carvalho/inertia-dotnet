using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inertia.Core;

public class InertiaService : IInertia
{
    private readonly InertiaOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Dictionary<string, object?> _sharedData = new();
    private string? _errorBag;

    public InertiaService(InertiaOptions options, IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _httpContextAccessor = httpContextAccessor;
        _errorBag = options.ErrorBag;
        
        foreach (var item in options.SharedData)
        {
            _sharedData[item.Key] = item.Value;
        }
    }

    public string? GetVersion()
    {
        if (_options.VersionResolver != null)
        {
            return _options.VersionResolver(_httpContextAccessor.HttpContext?.RequestServices!);
        }
        return _options.Version;
    }

    public Dictionary<string, object?> GetSharedData()
    {
        return new Dictionary<string, object?>(_sharedData);
    }

    public void Share(string key, object? value)
    {
        _sharedData[key] = value;
    }

    public void Share(Dictionary<string, object?> data)
    {
        foreach (var item in data)
        {
            _sharedData[item.Key] = item.Value;
        }
    }

    public InertiaResult Render(string component, Dictionary<string, object?>? props = null)
    {
        return new InertiaResult(component, props);
    }

    public void SetErrorBag(string? errorBag)
    {
        _errorBag = errorBag;
    }

    public string? GetErrorBag()
    {
        return _errorBag;
    }
}
