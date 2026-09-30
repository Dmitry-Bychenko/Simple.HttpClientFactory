using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Simple.HttpClientFactory.Test;

public sealed class DefaultHttpClientFactoryTest {
  [Fact]
  public async Task CreateClient_ValidHttpClient() {
    // Arrange
    using var server = WireMockServer.Start();

    server.Given(Request
      .Create()
      .WithPath("/test"))
      .RespondWith(Response.Create()
      .WithStatusCode(200)
      .WithBody("OK"));

    var factory = new DefaultHttpClientFactory();
    var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    Assert.True(response.IsSuccessStatusCode);
  }
}
