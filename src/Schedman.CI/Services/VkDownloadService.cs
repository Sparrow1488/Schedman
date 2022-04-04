using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using System.Threading.Tasks;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace Schedman.CI.Services
{
    internal class VkDownloadService : IVkDownloadService
    {
        public VkDownloadService(
            ILogger<VkAuthorizationService> logger, 
            VkManager vkManager)
        {
            _logger = logger;
            _vkManager = vkManager;
        }

        private readonly ILogger<VkAuthorizationService> _logger;
        private readonly VkManager _vkManager;

        private VkCollection<Video> _albumVideos;

        public async Task DownloadAlbumVideosAsync(string albumTitle)
        {
            _logger.LogInformation("Download FROM ALBUM => " + albumTitle);
            _albumVideos = await _vkManager.GetVideosFromAlbumAsync(albumTitle);
            if (_albumVideos.Count > 0)
            {
                _logger.LogDebug($"Found {_albumVideos.Count} videos in '{albumTitle}' album");
                _logger.LogInformation("Download started...");
                await _vkManager.DownloadVideosAsync(_albumVideos, albumTitle);
                _logger.LogInformation("Download completed");
            }
            else
            {
                _logger.LogWarning($"Not found any videos in '{albumTitle}' album");
            }
        }
    }
}
