namespace Plain.HttpClientFactory.Test;

public sealed class DefaultHttpClientFactoryTest {

  [Fact]
  public async Task CreateClient_ValidHttpClient() {
    // Arrange
    using var server = HttpTestHelper.CreateServer();

    var factory = new DefaultHttpClientFactory();

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    await HttpTestHelper.AssertResponse(response);
  }

  [Fact]
  public async Task CreateClient_SecondCreation_ValidHttpClient() {
    // Arrange
    using var server = HttpTestHelper.CreateServer();

    var factory = new DefaultHttpClientFactory();

    var first = factory.CreateClient("test");

    first.Dispose();

    using var client = factory.CreateClient("test");

    // Act
    var response = await client.GetAsync($"{server.Url}/test", TestContext.Current.CancellationToken);

    // Assert
    await HttpTestHelper.AssertResponse(response);

    var firstHandler = first.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;
    var secondHandler = client.GetType().GetField("_handler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(client) as SocketsHttpHandler;

    Assert.Equal(firstHandler, secondHandler);
  }
}
