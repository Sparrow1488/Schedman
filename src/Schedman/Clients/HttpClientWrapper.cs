using Polly;
using Polly.Retry;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Schedman.Clients
{
    internal class HttpClientWrapper : HttpClient
    {
        public HttpClientWrapper() : base()
        {
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                 .Or<WebException>()
                                 .WaitAndRetryAsync(3, sleepTime => TimeSpan.FromSeconds(1));
        }

        private readonly AsyncRetryPolicy _retryPolicy;

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return await _retryPolicy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
