namespace Simple.HttpClientFactory;

/// <summary>
/// A factory that creates <see cref="HttpClient"/> instances that share the same underlying <see cref="HttpMessageHandler"/>.
/// </summary>
public sealed class ClientHttpClientFactory : IHttpClientFactory {
  /// <summary>
  /// A <see cref="HttpMessageHandler"/> that forwards requests to the specified <see cref="HttpClient"/> instance.
  /// </summary>
  /// <param name="Inner">The <see cref="HttpClient"/> instance to forward requests to.</param>
  private sealed class ForwardingHandler(HttpClient Inner) : HttpMessageHandler {
    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
      Inner.SendAsync(MarkUnsent(request), cancellationToken);

    private static HttpRequestMessage MarkUnsent(HttpRequestMessage request) {
      var field = typeof(HttpRequestMessage)
        .GetField("_sendStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

      if (field is not null) {
        field.SetValue(request, 0);
      }

      return request;
    }

    public static async Task<HttpRequestMessage> Clone(HttpRequestMessage request) {
      var clone = new HttpRequestMessage(request.Method, request.RequestUri) {
        Version = request.Version
      };

      if (request.Content != null) {
        var ms = new MemoryStream();
        await request.Content.CopyToAsync(ms);
        ms.Position = 0;
        clone.Content = new StreamContent(ms);

        request.Content.Headers.ToList().ForEach(header => clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value));
      }

      request.Options.ToList().ForEach(option => clone.Options.TryAdd(option.Key, option.Value));

      request.Headers
          .ToList()
          .ForEach(header => clone.Headers.TryAddWithoutValidation(header.Key, header.Value));

      return clone;
    }
  }

  /// <summary>
  /// The underlying <see cref="HttpMessageHandler"/> that is shared by all created <see cref="HttpClient"/> instances.
  /// </summary>
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
  /// <returns>A new <see cref="HttpClient"/> instance.</returns>
  public HttpClient CreateClient(string name) => new(m_Handler, false);
}
