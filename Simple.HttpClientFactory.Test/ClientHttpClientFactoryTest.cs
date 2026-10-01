using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Simple.HttpClientFactory.Test;

public sealed class ClientHttpClientFactoryTest {

  [Fact]
  public async Task CreateClient_ValidHttpClient() {
    // Arrange
    using var server = CreateServer();
    var originalClient = new HttpClient();
    var factory = new ClientHttpClientFactory(originalClient);

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    Assert.True(response.IsSuccessStatusCode);
    Assert.Equal("OK", await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));

    var firstHandler = originalClient.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;
    var secondHandler = client.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;

    Assert.Equal(firstHandler, secondHandler);
  }

  private static WireMockServer CreateServer() {
    var server = WireMockServer.Start();

    server.Given(Request
      .Create()
      .WithPath("/test"))
      .RespondWith(Response.Create()
      .WithStatusCode(200)
      .WithBody("OK"));

    return server;
  }
}
