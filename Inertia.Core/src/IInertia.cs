using Microsoft.AspNetCore.Http;

namespace Inertia.Core;

public interface IInertia
{
    string? GetVersion();
    Dictionary<string, object?> GetSharedData();
    void Share(string key, object? value);
    void Share(Dictionary<string, object?> data);
    InertiaResult Render(string component, Dictionary<string, object?>? props = null);
    void SetErrorBag(string? errorBag);
    string? GetErrorBag();
}
