namespace Simple.HttpClientFactory.Test;

public sealed class HttpRequestMessageExtensionsTest
{
    [Fact]
    public async Task DeepClone()
    {
        // Arrange
        var message = new HttpRequestMessage();

        message.Version = new Version(1, 2, 3, 4);

        // Act
        var deepCopy = await message.DeepClone(CancellationToken.None);

        // Assert
        Assert.Equivalent(message, deepCopy);
    }
}

