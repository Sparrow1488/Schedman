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
            Timeout = TimeSpan.FromMinutes(1);
            
        }

        private readonly AsyncRetryPolicy _retryPolicy;

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return await _retryPolicy.ExecuteAsync(
                () => base.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken));
        }

        public async Task<byte[]> DownloadDataAsync(
            Uri url, IProgress<IntermediateProgressResult> downloadProgress = null)
        {
            using var response = await SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken: default);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            return memory.ToArray();
        }

        public async Task<byte[]> DownloadDataAsync(string url) =>
            await DownloadDataAsync(new Uri(url));
    }
}
