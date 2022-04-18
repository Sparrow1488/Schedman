using Schedman;
using Schedman.Extensions;
using Spectre.Console;
using System.Threading.Tasks;
using VkSchedman.Examples.Abstractions;

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
                var videoData = await _vkManager.DownloadVideoAsync(video);
                await _vkManager.SaveVideoLocalAsync(video, videoData, saveAlbumTitle: videosAlbumTitle);
            }
        }
    }
}
