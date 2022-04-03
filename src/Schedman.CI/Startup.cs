using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Schedman.CI
{
    internal class Startup : IHostedService
    {
        public Startup(
            IAuthorizationService authorization, 
            IVkDownloadService downloadService,
            ILogger<Startup> logger)
        {
            _authorization = authorization;
            _downloadService = downloadService;
            _logger = logger;
        }

        private readonly IAuthorizationService _authorization;
        private readonly IVkDownloadService _downloadService;
        private readonly ILogger<Startup> _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Schedman.CI started");
            await _authorization.AuthorizeAsync();
            await _downloadService.DownloadAlbumVideosAsync(GetVideosAlbumTitle());
        }

        public Task StopAsync(CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        private string GetVideosAlbumTitle() =>
            Environment.GetEnvironmentVariable("album");
    }
}
