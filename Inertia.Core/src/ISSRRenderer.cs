namespace Inertia.Core;

public interface ISSRRenderer
{
    Task<string?> RenderAsync(Page page);
}

public class DefaultSSRRenderer : ISSRRenderer
{
    public Task<string?> RenderAsync(Page page)
    {
        // Default implementation returns null - SSR requires a Node.js process
        // or similar to render React/Vue/Svelte components server-side.
        // Override this with a custom implementation that calls your SSR server.
        return Task.FromResult<string?>(null);
    }
}
