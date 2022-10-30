using Polly;
using Polly.Retry;
using Schedman.Tools.IO;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Schedman.Clients
{
    public class HttpClientWrapper : HttpClient
    {
        public HttpClientWrapper() : base()
        {
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                 .Or<WebException>()
                                 .WaitAndRetryAsync(3, sleepTime => TimeSpan.FromSeconds(2));
        }

        private readonly AsyncRetryPolicy _retryPolicy;

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return await _retryPolicy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }

        public async Task<byte[]> DownloadDataAsync(
            Uri url, IProgress<IntermediateProgressResult> downloadProgress = null)
        {
            var response = await SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken: default);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> DownloadDataAsync(string url) =>
            await DownloadDataAsync(new Uri(url));
    }
}
