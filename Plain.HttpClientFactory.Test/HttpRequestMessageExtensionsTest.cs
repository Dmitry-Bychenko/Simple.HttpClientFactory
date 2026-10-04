namespace Simple.HttpClientFactory.Test;

public sealed class HttpRequestMessageExtensionsTest {
  [Fact]
  public async Task DeepClone() {
    // Arrange
    var message = CreateTestMessage();

    // Act
    var deepCopy = await message.DeepClone(CancellationToken.None);

    // Assert
    Assert.Equivalent(message, deepCopy);
  }

  private static HttpRequestMessage CreateTestMessage() {
    var message = new HttpRequestMessage() {
      Version = new Version(1, 2, 3, 4),
      Content = new StringContent("Test content"),
      Headers = {
        { "X-Test-Header", "TestValue X" },
        { "Y-Test-Header", "TestValue Y" },
      }
    };

    message.Options.Set(new HttpRequestOptionsKey<string>("TestOption"), "TestOptionValue");

    return message;
  }
}

