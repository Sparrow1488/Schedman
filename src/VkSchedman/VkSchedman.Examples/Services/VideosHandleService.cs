using Serilog;
using Spectre.Console;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Examples.Abstractions;

namespace VkSchedman.Examples.Services
{
    internal sealed class VideosHandleService : IStartableService
    {
        public VideosHandleService(VkManager vkManager)
        {
            if (vkManager.IsAuthorizated)
                _vkManager = vkManager;
            else throw new VkAuthorizationException("VkManager not authorizated!");
        }

        private readonly VkManager _vkManager;

        public async Task StartAsync()
        {
            Log.Information($"{nameof(VideosHandleService)} started");
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
                Log.Information($"[{i + 1}/{videos.Count}] Start Download " + video.Title);
                var videoData = await _vkManager.DownloadVideoAsync(video);
                await _vkManager.SaveVideoLocalAsync(video, videoData);
                Log.Information($"[{i + 1}/{videos.Count}] Downloaded");
            }
            Log.Information("Videos downloaded");
        }
    }
}
