# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-XX

### Added
- **DefaultHttpClientFactory** - Default implementation of `IHttpClientFactory` with shared `SocketsHttpHandler`
  - Automatic decompression (gzip, deflate, brotli)
  - Shared cookie container
  - Default credentials support
  - Configurable connection pooling (100 seconds default lifetime)
  - Thread-safe lazy initialization pattern

- **ClientHttpClientFactory** - Factory for wrapping existing `HttpClient` instances
  - Creates new clients that share the underlying handler
  - Custom `ForwardingHandler` for request forwarding
  - Reflection-based request state management with graceful fallback

- **Extension Methods** - Convenient static helpers
  - `DefaultFactory` property for accessing the default singleton
  - `FactoryFromClient()` method for wrapping existing clients

- **Comprehensive Unit Tests**
  - Full test coverage using xUnit and WireMock.Net
  - DefaultHttpClientFactory tests
  - ClientHttpClientFactory tests
  - Extension method tests
  - Request/response helper tests

- **Documentation**
  - Comprehensive README.md with examples and API reference
  - XML documentation on all public types
  - WireMock.Net based test helper utilities

- **NuGet Package**
  - Source Link support for debugging
  - Symbol package (.snupkg) for symbol server
  - Package icon and metadata
  - MIT License

### Technical Details
- Target Framework: .NET 10.0
- Language Version: Latest
- Nullable Reference Types: Enabled
- Code Style: Enforced in build
- Compiler Warnings: Treated as errors

## Version History

### Planned for Future Releases
- Additional handler implementations
- Performance optimizations
- Extended middleware support
- Resilience patterns integration

---

**Repository:** https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory  
**Issues:** https://github.com/Dmitry-Bychenko/Simple.HttpClientFactory/issues  
**License:** MIT
