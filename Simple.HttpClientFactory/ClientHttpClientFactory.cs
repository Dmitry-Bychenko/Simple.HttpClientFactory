namespace Simple.HttpClientFactory;

/// <summary>
/// A factory that creates <see cref="HttpClient"/> instances that share the same underlying <see cref="HttpMessageHandler"/>.
/// </summary>
public sealed class ClientHttpClientFactory : IHttpClientFactory {
  private sealed class ForwardingHandler(HttpClient Inner) : HttpMessageHandler {
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
      Inner.SendAsync(request, cancellationToken);
  }

  private readonly HttpMessageHandler m_Handler;

  /// <summary>
  /// Creates a new instance of <see cref="ClientHttpClientFactory"/> that uses the specified <see cref="HttpClient"/> as the underlying handler for all created clients.
  /// </summary>
  /// <param name="originalClient">The <see cref="HttpClient"/> to use as the underlying handler.</param>
  public ClientHttpClientFactory(HttpClient originalClient) {
    ArgumentNullException.ThrowIfNull(originalClient);

    m_Handler = new ForwardingHandler(originalClient);
  }

  /// <summary>
  /// Creates a new <see cref="HttpClient"/> instance that shares the same underlying <see cref="HttpMessageHandler"/> as the original client.
  /// </summary>
  /// <param name="name">The name of the client to create. This parameter is ignored.</param>
  public HttpClient CreateClient(string name) => new(m_Handler, false);
}
