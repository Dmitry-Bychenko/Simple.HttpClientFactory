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
var client = factory.CreateClient("myClient");
var response = await client.GetAsync("https://example.com");
```

### 2. **ClientHttpClientFactory**
Wraps an existing `HttpClient` instance and creates new clients that share its underlying handler. Useful for scenarios where you have a pre-configured client.

```csharp
var originalClient = new HttpClient();
var factory = new ClientHttpClientFactory(originalClient);
var newClient = factory.CreateClient("derived");
// Both clients share the same handler
```

### 3. **Extension Methods**
Convenient extension methods for working with `IHttpClientFactory`:

```csharp
// Get the default factory
var factory = IHttpClientFactory.DefaultFactory;

// Create a factory from an existing client
var myClient = new HttpClient();
var factory = myClient.FactoryFromClient();
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

## Quick Start

### Basic Usage

```csharp
using Simple.HttpClientFactory;

// Create a simple HTTP client factory
var factory = new DefaultHttpClientFactory();

// Create HTTP clients as needed
var client1 = factory.CreateClient("api-client");
var client2 = factory.CreateClient("webhook-client");

// Use normally
var response = await client1.GetAsync("https://api.example.com/data");
var content = await response.Content.ReadAsStringAsync();
```

### With Existing HttpClient

```csharp
// Configure a client with custom settings
var configuredClient = new HttpClient();
configuredClient.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
configuredClient.DefaultRequestHeaders.Add("Authorization", "Bearer token123");

// Wrap it in a factory
var factory = new ClientHttpClientFactory(configuredClient);

// All created clients inherit the configuration
var derivedClient = factory.CreateClient("service");
```

### Dependency Injection (if needed)

```csharp
// Register in your DI container
services.AddSingleton<IHttpClientFactory>(new DefaultHttpClientFactory());

// Inject into your services
public class MyService {
    public MyService(IHttpClientFactory factory) {
        _client = factory.CreateClient("my-service");
    }
}
```

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

Modify by creating a custom implementation:

```csharp
public class CustomHttpClientFactory : IHttpClientFactory {
    public HttpClient CreateClient(string name) {
        var handler = new SocketsHttpHandler {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            // ... other settings
        };
        return new HttpClient(handler);
    }
}
```

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

❌ **Consider Alternatives For:**
- Large enterprise applications with complex middleware
- Advanced resilience patterns (Polly integration in HttpClientFactory)
- Multiple named clients with DI

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

## Performance Considerations

- **Handler Reuse**: Both implementations reuse the underlying `HttpMessageHandler`, reducing memory allocation
- **Lazy Initialization**: `DefaultHttpClientFactory` uses `Lazy<T>` for thread-safe singleton pattern
- **Connection Pooling**: Default 100-second connection lifetime balances resource usage with performance
- **No Reflection Overhead**: Extension methods are compile-time optimized

## Troubleshooting

### Issue: "Cannot access private field '_sendStatus'"

**Solution**: The `ClientHttpClientFactory` gracefully falls back to deep-cloning the request if reflection is unavailable (e.g., with AOTC). This is handled automatically.

### Issue: Connections timing out

**Solution**: Adjust the connection lifetime:
```csharp
public class CustomFactory : IHttpClientFactory {
    public HttpClient CreateClient(string name) {
        var handler = new SocketsHttpHandler {
            PooledConnectionLifetime = TimeSpan.FromSeconds(60)
        };
        return new HttpClient(handler);
    }
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

## Project Structure

```
Simple.HttpClientFactory/
├── DefaultHttpClientFactory.cs          # Default implementation
├── ClientHttpClientFactory.cs           # Client wrapping implementation
├── HttpClientFactoryExtensions.cs       # Extension methods
├── HttpRequestMessageExtensions.cs      # Helper extensions
└── icon.png                             # Package icon

Simple.HttpClientFactory.Test/
├── DefaultHttpClientFactoryTest.cs      # Default factory tests
├── ClientHttpClientFactoryTest.cs       # Client factory tests
├── HttpClientFactoryExtensionsTest.cs   # Extension method tests
├── HttpRequestMessageExtensionsTest.cs  # Helper tests
└── HttpTestHelper.cs                    # WireMock test utilities
```

## License

This project is licensed under the MIT License.

## Author

**Dmitry Bychenko** - [GitHub Profile](https://github.com/Dmitry-Bychenko)

## Support

For issues, questions, or suggestions, please open an issue on GitHub.

---

**Made with ❤️ for the .NET community**