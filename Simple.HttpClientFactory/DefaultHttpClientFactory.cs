using System.Net;

namespace Simple.HttpClientFactory;

/// <summary>
/// A default implementation of <see cref="IHttpClientFactory"/> that creates <see cref="HttpClient"/> instances with a shared <see cref="SocketsHttpHandler"/>.
/// </summary>
public sealed class DefaultHttpClientFactory : IHttpClientFactory {
  private static readonly Lazy<SocketsHttpHandler> HttpClientHandlerBuilder = new(() => new() {
    AutomaticDecompression = DecompressionMethods.All,
    CookieContainer = new CookieContainer(),
    Credentials = CredentialCache.DefaultCredentials,
    PooledConnectionLifetime = ConnectionLifeTime
  });

  /// <summary>
  /// Connection lifetime for the underlying <see cref="SocketsHttpHandler"/>. This is set to 100 seconds to match the default connection lifetime of <see cref="HttpClient"/> instances.
  /// </summary>
  public static readonly TimeSpan ConnectionLifeTime = TimeSpan.FromSeconds(100);

  /// <summary>
  /// Creates a new <see cref="HttpClient"/> instance with a shared <see cref="SocketsHttpHandler"/>.
  /// </summary>
  /// <param name="name">The name of the client to create. This parameter is ignored.</param>
  /// <returns>A new <see cref="HttpClient"/> instance.</returns>
  public HttpClient CreateClient(string name) => new(HttpClientHandlerBuilder.Value, false);
}
