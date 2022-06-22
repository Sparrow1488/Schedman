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

        public async Task<byte[]> DownloadDataAsync(Uri url, IProgress<IntermediateProgressResult> downloadProgress = null)
        {
            var response = await this.SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken: default);
            response.EnsureSuccessStatusCode();
            var responseLength = response.Content.Headers.ContentLength;
            var buffer = new byte[(int)responseLength];
            int onePercent = buffer.Length / 100;
            var progressResult = new IntermediateProgressResult();

            using (var ms = new MemoryStream())
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    bool dataAviable = true;
                    do
                    {
                        var readByte = responseStream.ReadByte();
                        ms.WriteByte((byte)readByte);
                        if (readByte != -1)
                        {
                            if (ms.Length % 1_500_000 == 0)
                            {
                                if (downloadProgress != null)
                                {
                                    double progressPercentage = (double)ms.Length / onePercent;

                                    progressResult.TotalSize = (int)responseLength.Value;
                                    progressResult.CurrentPercents = progressPercentage;
                                    progressResult.CurrentSize = (int)ms.Length;

                                    downloadProgress.Report(progressResult);
                                }
                            }
                        }
                        else
                        {
                            dataAviable = false;
                            progressResult.CurrentPercents = 100;
                            progressResult.CurrentSize = (int)responseLength;
                            downloadProgress?.Report(progressResult);
                        }
                    }
                    while (dataAviable);
                }
                buffer = ms.ToArray();
            }
            
            return buffer;
        }

        public async Task<byte[]> DownloadDataAsync(string url) =>
            await DownloadDataAsync(new Uri(url));
    }
}
