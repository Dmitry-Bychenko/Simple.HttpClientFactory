using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Simple.HttpClientFactory.Test;

public sealed class DefaultHttpClientFactoryTest {

  [Fact]
  public async Task CreateClient_ValidHttpClient() {
    // Arrange
    using var server = CreateServer();

    var factory = new DefaultHttpClientFactory();

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    Assert.True(response.IsSuccessStatusCode);
    Assert.Equal("OK", await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
  }

  [Fact]
  public async Task CreateClient_SecondCreation_ValidHttpClient() {
    // Arrange
    using var server = CreateServer();

    var factory = new DefaultHttpClientFactory();

    var first = factory.CreateClient("test");

    first.Dispose();

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    Assert.True(response.IsSuccessStatusCode);
    Assert.Equal("OK", await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));

    var firstHandler = first.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;
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
