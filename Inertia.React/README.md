# Inertia.React

[![NuGet](https://img.shields.io/nuget/v/Inertia.React.svg)](https://www.nuget.org/packages/Inertia.React/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> React adapter for Inertia.js on ASP.NET Core

Build modern React applications with server-side rendering support using Inertia.js and ASP.NET Core.

## Installation

```bash
dotnet add package Inertia.React
```

## Quick Start

### 1. Configure Services

```csharp
using Inertia.React;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInertiaReact(options =>
{
    options.Version = "1.0.0";
    options.RootView = "_Inertia";
});

var app = builder.Build();
app.UseInertia();
app.MapControllers();
app.Run();
```

### 2. Create React Components

Create `wwwroot/js/pages/Home/Index.jsx`:

```jsx
import React from 'react';

export default function Index({ message, users }) {
  return (
    <div>
      <h1>{message}</h1>
      <ul>
        {users.map(user => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
    </div>
  );
}
```

### 3. Initialize Inertia App

Create `wwwroot/js/app.jsx`:

```jsx
import { createInertiaApp } from '@inertiajs/react';
import { createRoot } from 'react-dom/client';

const pages = import.meta.glob('./pages/**/*.jsx', { eager: true });

const resolve = (name) => {
  return pages[`./pages/${name}.jsx`]?.default;
};

createInertiaApp({
  resolve,
  setup({ el, App, props }) {
    createRoot(el).render(<App {...props} />);
  },
});
```

## Server-Side Rendering (SSR)

Enable SSR for better initial load times and SEO:

```csharp
builder.Services.AddInertiaReact(options =>
{
    options.EnableSSR = true;
    options.SSREndpoint = "http://localhost:13714/render";
});
```

Create an SSR server using Node.js:

```javascript
// ssr.js
import { createInertiaApp } from '@inertiajs/react';
import createServer from '@inertiajs/react/server';
import ReactDOMServer from 'react-dom/server';

const pages = import.meta.glob('./pages/**/*.jsx', { eager: true });

createServer((page) =>
  createInertiaApp({
    page,
    resolve: (name) => pages[`./pages/${name}.jsx`],
    render: ReactDOMServer.renderToString,
    setup({ App, props }) {
      return <App {...props} />;
    },
  })
);
```

## Features

- **React Integration** - Full React support with hooks and components
- **SSR Support** - Built-in server-side rendering via HTTP endpoint
- **Component Auto-Discovery** - Automatic component resolution by name
- **TypeScript Support** - Included type definitions for Inertia page props

## Project Structure

```
Inertia.React/
├── src/
│   ├── ReactSSRRenderer.cs      # HTTP-based SSR implementation
│   └── ReactInertiaExtensions.cs # Service registration extensions
├── contentFiles/
│   └── js/
│       └── types.d.ts           # TypeScript definitions
└── Inertia.React.csproj
```

## TypeScript Support

The package includes TypeScript definitions for Inertia page data:

```typescript
import { PageProps } from '@inertiajs/react';

interface DashboardProps extends PageProps {
  user: { name: string };
  stats: { users: number };
}

export default function Dashboard({ user, stats }: DashboardProps) {
  // ...
}
```

## Testing

Run the test suite:

```bash
dotnet test tests/Inertia.React.Tests
```

## License

MIT License - see [LICENSE](LICENSE) for details.

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details.

## Related Packages

- [Inertia.Core](https://www.nuget.org/packages/Inertia.Core/) - Core Inertia.js adapter

## Support

- [Inertia.js Documentation](https://inertiajs.com/)
- [React Documentation](https://react.dev/)
- [GitHub Issues](https://github.com/yourusername/inertia-dotnet/issues)
