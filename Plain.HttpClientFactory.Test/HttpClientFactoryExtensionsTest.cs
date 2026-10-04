namespace Plain.HttpClientFactory.Test;

public sealed class HttpClientFactoryExtensionsTest {
  [Fact]
  public void DefaultFactory_ShouldReturnDefaultHttpClientFactory() {
    // Act
    var factory = IHttpClientFactory.DefaultFactory;

    // Assert
    Assert.IsType<DefaultHttpClientFactory>(factory);
  }

  [Fact]
  public void FactoryFromClient_NotNullOriginal_ClientHttpClientFactory() {
    // Arrange
    var originalClient = new HttpClient();

    // Act
    var factory = IHttpClientFactory.FactoryFromClient(originalClient);

    // Assert
    Assert.IsType<ClientHttpClientFactory>(factory);
  }

  [Fact]
  public void FactoryFromClient_NullOriginal_DefaultHttpClientFactory() {
    // Act
    var factory = IHttpClientFactory.FactoryFromClient(null);

    // Assert
    Assert.IsType<DefaultHttpClientFactory>(factory);
  }
}
