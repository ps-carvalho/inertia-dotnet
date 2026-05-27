namespace Inertia.Core;

public class InertiaOptions
{
    public string RootView { get; set; } = "_Inertia";
    public string? Version { get; set; }
    public Func<IServiceProvider, string?>? VersionResolver { get; set; }
    public Dictionary<string, object?> SharedData { get; set; } = new();
    public bool EnableSSR { get; set; } = false;
    public string? SSREndpoint { get; set; }
    public string? ErrorBag { get; set; }
}
