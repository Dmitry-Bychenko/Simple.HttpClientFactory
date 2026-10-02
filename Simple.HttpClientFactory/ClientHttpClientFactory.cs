using System.Reflection;

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
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
      await Inner.SendAsync(await MarkUnsent(request), cancellationToken);

    private static readonly FieldInfo? SendStatusField = typeof(HttpRequestMessage)
      .GetField("_sendStatus", BindingFlags.NonPublic | BindingFlags.Instance);

    private static async ValueTask<HttpRequestMessage> MarkUnsent(HttpRequestMessage request) {
      if (SendStatusField is not null) {
        SendStatusField.SetValue(request, 0);

        return request;
      }

      // If we can't access the private field, we can create a deep clone of the request instead.
      return await request.DeepClone();
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
