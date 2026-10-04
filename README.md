# Intuition

As we know http connection usage is [quite complex](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines) in c#: 
1. If we create `HttpClient` on each connection (`HttpClient` is not a sigleton) we have ports leakage
2. If `HttpClient` is a sigleton, we din't react on DNS changes. It can be OK for command line utility, but not for ASP program.
3. We can use `IHttpClientFactory`, but it is an overshoot for a simple routine when we want to connect and perform a single query.

This simple package is created to make http creation easier, especially when we want to create a library class which wants http connection:
```
public class MyLibraryClass {
  private readonly m_ConnectionFactory;

  // If we want to create library class instance within ASP we provide connection factory
  public MyLibraryClass(IHttpConnectionFactory connectionFactory) {
    m_ConnectionFactory = connectionFactory;
  }

  // If we want to create library class instance for a standalone routine which provides singleton connection
  public MyLibraryClass(IHttpConnection connection) {
    m_ConnectionFactory = IHttpClientFactory.FactoryFromClient(connection);
  }

  //  If we want to create library class instance for a standalone routine which has no predefined connection
  public MyLibraryClass() {
    m_ConnectionFactory = IHttpClientFactory.DefaultFactory;
  }

  ...

  private async Task Perform() {
    // Business as usual
    using var http = m_ConnectionFactory.CreateClient();
    ...
  }  
}
```

# Simple.HttpClientFactory

A lightweight and simple implementation of `IHttpClientFactory` for .NET that provides flexible HTTP client creation with shared handler management.

![License](https://img.shields.io/badge/license-MIT-green)
![.NET Version](https://img.shields.io/badge/.NET-10.0-blue)

## Overview

**Simple.HttpClientFactory** is a minimal NuGet package that offers alternative implementations of `IHttpClientFactory` for scenarios where you need simple, predictable HTTP client creation without the complexity of full dependency injection setup.

This package solves common HTTP client management challenges:
- ✅ Shared `SocketsHttpHandler` with proper pooling and decompression
- ✅ Connection lifetime management (100 seconds default)
- ✅ Support for wrapping existing `HttpClient` instances
- ✅ Cookie and credential handling
- ✅ Fully unit tested with WireMock.Net

## Features

### 1. **DefaultHttpClientFactory**
Creates HTTP clients with a shared `SocketsHttpHandler` that includes:
- Automatic decompression (gzip, deflate, brotli)
- Shared cookie container
- Default credentials support
- Configurable connection pooling (100 seconds lifetime)

```csharp
var factory = new DefaultHttpClientFactory();
using var client = factory.CreateClient("myClient");
var response = await client.GetAsync("https://example.com");
```

### 2. **ClientHttpClientFactory**
Wraps an existing `HttpClient` instance and creates new clients that share its underlying handler. Useful for scenarios where you have a pre-configured client.

```csharp
var originalClient = new HttpClient();
...
var factory = new ClientHttpClientFactory(originalClient);

using var newClient = factory.CreateClient("derived");
// Both clients share the same handler
```

### 3. **Extension Methods**
Convenient extension methods for working with `IHttpClientFactory`:

```csharp
// Get the default factory
var factory = IHttpClientFactory.DefaultFactory;

// Create a factory from an existing client
var myClient = new HttpClient();
...
var factory = IHttpClientFactory.FactoryFromClient(myClient);
```

## Installation

**NuGet Package Manager:**
```bash
Install-Package Simple.HttpClientFactory
```

**.NET CLI:**
```bash
dotnet add package Simple.HttpClientFactory
```

Or add directly to your `.csproj`:
```xml
<ItemGroup>
  <PackageReference Include="Simple.HttpClientFactory" Version="*" />
</ItemGroup>
```

## Requirements

- .NET 10.0 or higher
- Microsoft.Extensions.Http 10.0.12 or compatible

## Architecture

### Class Hierarchy

```
IHttpClientFactory (interface)
├── DefaultHttpClientFactory
│   └── Uses shared SocketsHttpHandler with automatic pooling
└── ClientHttpClientFactory
    └── Wraps an existing HttpClient with ForwardingHandler
```

### Key Components

- **DefaultHttpClientFactory**: Thread-safe singleton pattern with lazy-initialized handler
- **ClientHttpClientFactory**: Wraps existing clients using a custom `ForwardingHandler`
- **ForwardingHandler**: Internal message handler that forwards requests while managing request state
- **HttpClientFactoryExtensions**: Utility methods for factory creation and access

## Configuration

### DefaultHttpClientFactory Settings

The default factory uses these settings:

| Setting | Value | Purpose |
|---------|-------|---------|
| Target Framework | .NET 10.0 | Latest .NET version support |
| Connection Lifetime | 100 seconds | Connection pooling timeout |
| Auto Decompression | All | gzip, deflate, brotli support |
| Credentials | Default | Uses system credentials |
| Cookie Container | Shared | Maintains cookies across requests |

## Testing

The project includes comprehensive unit tests using **xUnit** and **WireMock.Net**:

```bash
dotnet test
```

### Test Coverage

- ✅ Default HTTP client creation
- ✅ Shared handler verification
- ✅ Client wrapping functionality
- ✅ Extension method behavior
- ✅ Mock HTTP request/response handling

## Differences from Microsoft.Extensions.Http

| Feature | Simple.HttpClientFactory | Microsoft.Extensions.Http |
|---------|--------------------------|--------------------------|
| Setup | Minimal, no DI needed | Requires full DI setup |
| Handler Sharing | Automatic (Lazy<T>) | Manual or DI-based |
| Size | Lightweight | Full-featured |
| Scenarios | Simple to moderate | Complex enterprise |
| Learning Curve | Very low | Moderate |

## Use Cases

✅ **Best For:**
- Small to medium applications
- Scenarios without dependency injection
- Quick prototyping
- Microservices with simple HTTP needs
- Legacy system integration
- Testing and mocking scenarios

## API Reference

### DefaultHttpClientFactory

```csharp
public sealed class DefaultHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name);
    public static TimeSpan ConnectionLifeTime { get; }
}
```

### ClientHttpClientFactory

```csharp
public sealed class ClientHttpClientFactory : IHttpClientFactory
{
    public ClientHttpClientFactory(HttpClient originalClient);
    public HttpClient CreateClient(string name);
}
```

### Extension Methods

```csharp
public static class HttpClientFactoryExtensions
{
    public static IHttpClientFactory DefaultFactory { get; }
    public static IHttpClientFactory FactoryFromClient(HttpClient? originalClient);
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Setup

```bash
# Clone the repository
git clone https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory.git
cd Simple.HttpClientFactory

# Restore dependencies
dotnet restore

# Run tests
dotnet test

# Build
dotnet build

# Pack NuGet
dotnet pack -c Release
```

## License

This project is licensed under the MIT License.

## Author

**Dmitry Bychenko** - [GitHub Profile](https://github.com/Dmitry-Bychenko)

## Support

For issues, questions, or suggestions, please open an issue on GitHub.

---

**Made with ❤️ for the .NET community**
