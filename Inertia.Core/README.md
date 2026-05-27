# Inertia.Core

[![NuGet](https://img.shields.io/nuget/v/Inertia.Core.svg)](https://www.nuget.org/packages/Inertia.Core/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> The official Inertia.js server-side adapter for ASP.NET Core

Create modern single-page applications using classic server-side routing and controllers. No APIs required.

## What is Inertia?

Inertia.js lets you build single-page apps (SPAs) without building an API. You write controllers like you normally would, and Inertia handles the client-side routing and rendering automatically.

## Installation

```bash
dotnet add package Inertia.Core
```

## Quick Start

### 1. Register Services

```csharp
using Inertia.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInertia(options =>
{
    options.Version = "1.0.0";
    options.RootView = "_Inertia";
});

var app = builder.Build();
app.UseInertia();
app.MapControllers();
app.Run();
```

### 2. Create Root View

Create `Views/Shared/_Inertia.cshtml`:

```html
@using Inertia.Core
@{
    Layout = null;
    var page = ViewData["page"] as Page;
    var pageJson = ViewData["pageJson"] as string ?? "{}";
}
<!DOCTYPE html>
<html>
<head>
    <title>@(page?.Component ?? "Inertia App")</title>
</head>
<body>
    <div id="app" data-page='@Html.Raw(pageJson)'></div>
    <script>window.__INERTIA_PAGE__ = @Html.Raw(pageJson);</script>
</body>
</html>
```

### 3. Return Inertia Responses

```csharp
public class HomeController : Controller
{
    private readonly IInertia _inertia;

    public HomeController(IInertia inertia)
    {
        _inertia = inertia;
    }

    public IActionResult Index()
    {
        _inertia.Share("appName", "My App");

        return this.Inertia("Home/Index", new Dictionary<string, object?>
        {
            ["message"] = "Hello from Inertia!"
        });
    }
}
```

## Features

- **Server-Side Rendering (SSR)** - Built-in SSR support with configurable renderers
- **Lazy Props** - Defer expensive data loading until needed
- **Deferred Props** - Load data on subsequent requests
- **Partial Reloads** - Only fetch the data you need with `only`/`except`
- **Validation Errors** - Automatic ModelState integration with Inertia-compatible format
- **Versioning** - Automatic asset version checking to handle deployments
- **Shared Data** - Share data across all responses

## Configuration

```csharp
builder.Services.AddInertia(options =>
{
    options.RootView = "_Inertia";        // Root Razor view
    options.Version = "1.0.0";            // Static version string
    options.EnableSSR = true;             // Enable server-side rendering
    options.SSREndpoint = "http://...";   // SSR server endpoint
    
    // Or use a version resolver
    options.VersionResolver = _ => File.GetLastWriteTime("wwwroot/dist").Ticks.ToString();
});
```

## Lazy & Deferred Props

```csharp
public IActionResult Dashboard()
{
    return this.Inertia("Dashboard/Index", new Dictionary<string, object?>
    {
        // Always included
        ["user"] = _db.Users.Find(userId),
        
        // Only resolved when explicitly requested
        ["posts"] = InertiaExtensions.Lazy(() => _db.Posts.ToList()),
        
        // Resolved only when 'stats' group is requested
        ["stats"] = InertiaExtensions.Deferred("stats", () => GetStats())
    });
}
```

## Validation

```csharp
[HttpPost]
public IActionResult Store([FromBody] CreateRequest request)
{
    this.InertiaValidate(ModelState, "createForm");
    
    // Validation passed...
}
```

The validation errors will be returned in Inertia-compatible format with a 422 status code.

## Project Structure

```
Inertia.Core/
├── src/
│   ├── IInertia.cs              # Main service interface
│   ├── InertiaService.cs        # Core implementation
│   ├── InertiaResult.cs         # ActionResult for Inertia responses
│   ├── InertiaMiddleware.cs     # Request/response handling
│   ├── InertiaExtensions.cs     # Service registration & helpers
│   ├── InertiaOptions.cs        # Configuration options
│   ├── Page.cs                  # Inertia page data model
│   ├── InertiaConstants.cs      # Header names & helpers
│   ├── LazyProp.cs              # Lazy prop wrapper
│   ├── InertiaValidationException.cs  # Validation handling
│   └── ISSRRenderer.cs          # SSR interface
└── Inertia.Core.csproj
```

## Testing

Run the test suite:

```bash
dotnet test tests/Inertia.Core.Tests
```

## License

MIT License - see [LICENSE](LICENSE) for details.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details.

## Support

- [Documentation](https://inertiajs.com/)
- [GitHub Issues](https://github.com/yourusername/inertia-dotnet/issues)
- [Discord](https://discord.gg/inertiajs)
