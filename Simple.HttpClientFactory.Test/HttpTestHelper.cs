using System;
using System.Collections.Generic;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Simple.HttpClientFactory.Test;

/// <summary>
/// 
/// </summary>
public static class HttpTestHelper
{
    public const string BodyText = "OK text";

    public static WireMockServer CreateServer()
    {
        var server = WireMockServer.Start();

        server.Given(Request
          .Create()
          .WithPath("/test"))
          .RespondWith(Response.Create()
          .WithStatusCode(200)
          .WithBody(BodyText));

        return server;
    }

    public static async Task AssertResponse(HttpResponseMessage? response)
    {
        Assert.NotNull(response);

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(BodyText, await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }
}

