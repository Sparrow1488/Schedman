using Spectre.Console;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Examples.Abstractions;
using VkSchedman.Examples.Entities;
using VkSchedman.Extensions;

namespace VkSchedman.Examples.Services
{
    internal sealed class VideosHandleService : IStartableService
    {
        public VideosHandleService(VkManager vkManager)
        {
            vkManager.ThrowIfNotAuth();
            _vkManager = vkManager;
        }

        private readonly VkManager _vkManager;

        public async Task StartAsync()
        {
            Logger.Info($"{nameof(VideosHandleService)} started");
            var serviceChapters = new[] { nameof(DownloadOwnVideos) };
            var serviceChapter = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                                    .Title("Select service chapter: ")
                                                    .AddChoices(serviceChapters));
            if (serviceChapter == nameof(DownloadOwnVideos))
                await DownloadOwnVideos();
        }

        private async Task DownloadOwnVideos()
        {
            string videosAlbumTitle = AnsiConsole.Ask<string>("Album title: ");
            var videos = await _vkManager.GetVideosFromAlbumAsync(videosAlbumTitle, count: 1000);
            for (int i = 0; i < videos.Count; i++)
            {
                var video = videos[i];
                Logger.Info($"[{i + 1}/{videos.Count}] Starting Download " + video.Title);
                var videoData = await _vkManager.DownloadVideoAsync(video);
                await _vkManager.SaveVideoLocalAsync(video, videoData, saveAlbumTitle: videosAlbumTitle);
            }
            Logger.Info("Videos downloaded");
        }
    }
}
