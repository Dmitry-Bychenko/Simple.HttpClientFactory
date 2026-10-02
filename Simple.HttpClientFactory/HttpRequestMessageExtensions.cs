namespace Simple.HttpClientFactory;

/// <summary>
/// 
/// </summary>
public static class HttpRequestMessageExtensions
{
    extension(HttpRequestMessage request)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HttpRequestMessage> DeepClone(CancellationToken cancellationToken = default)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            if (request.Content is not null)
            {
                var ms = new MemoryStream();

                try
                {
                    await request.Content.CopyToAsync(ms, cancellationToken);

                    ms.Position = 0;
                    clone.Content = new StreamContent(ms);
                }
                catch
                {
                    await ms.DisposeAsync();

                    throw;
                }

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var option in request.Options)
                clone.Options.TryAdd(option.Key, option.Value);

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}
