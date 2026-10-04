namespace Plain.HttpClientFactory.Test;

public sealed class ClientHttpClientFactoryTest {

  [Fact]
  public async Task CreateClient_ValidHttpClient() {
    // Arrange
    using var server = HttpTestHelper.CreateServer();
    var originalClient = new HttpClient();
    var factory = new ClientHttpClientFactory(originalClient);

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    await HttpTestHelper.AssertResponse(response);

    var firstHandler = originalClient.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;
    var secondHandler = client.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;

    Assert.Equal(firstHandler, secondHandler);
  }
}
