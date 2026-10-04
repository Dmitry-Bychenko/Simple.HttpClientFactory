using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Simple.HttpClientFactory.Test;

/// <summary>
/// Provides helper methods for testing HTTP clients using WireMock.Net.
/// </summary>
public static class HttpTestHelper {
  /// <summary>
  /// The body text to be returned in the mock HTTP response.
  /// </summary>
  public const string BodyText = "OK text";

  /// <summary>
  /// Creates and starts a WireMock server with a predefined endpoint that responds with a 200 status code and a specific body text.
  /// </summary>
  public static WireMockServer CreateServer() {
    var server = WireMockServer.Start();

    server.Given(Request
      .Create()
      .WithPath("/test"))
      .RespondWith(Response.Create()
      .WithStatusCode(200)
      .WithBody(BodyText));

    return server;
  }

  /// <summary>
  /// Asserts that the provided <see cref="HttpResponseMessage"/> is not null, has a successful status code, and contains the expected body text.
  /// </summary>
  /// <param name="response">Response to validate</param>
  public static async Task AssertResponse(HttpResponseMessage? response) {
    Assert.NotNull(response);

    Assert.True(response.IsSuccessStatusCode);
    Assert.Equal(BodyText, await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
  }
}

