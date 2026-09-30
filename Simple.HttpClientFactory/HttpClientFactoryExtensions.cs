namespace Simple.HttpClientFactory;

/// <summary>
/// Provides extension methods for <see cref="IHttpClientFactory"/>.
/// </summary>
public static class HttpClientFactoryExtensions {
  private static readonly DefaultHttpClientFactory s_DefaultFactory = new();

  extension(IHttpClientFactory) {
    /// <summary>
    /// Gets a default implementation of <see cref="IHttpClientFactory"/> that creates <see cref="HttpClient"/> instances with a shared <see cref="SocketsHttpHandler"/>.
    /// </summary>
    public static DefaultHttpClientFactory DefaultFactory => s_DefaultFactory;

    /// <summary>
    /// Creates an <see cref="IHttpClientFactory"/> that returns the specified <see cref="HttpClient"/> instance for all requests.
    /// </summary>
    /// <param name="originalClient">The <see cref="HttpClient"/> instance to return for all requests.</param>
    /// <returns>An <see cref="IHttpClientFactory"/> that returns the specified <see cref="HttpClient"/> instance for all requests.</returns>
    public static ClientHttpClientFactory CreateFromClient(HttpClient originalClient) {
      ArgumentNullException.ThrowIfNull(originalClient);

      return new ClientHttpClientFactory(originalClient);
    }
  }
}
