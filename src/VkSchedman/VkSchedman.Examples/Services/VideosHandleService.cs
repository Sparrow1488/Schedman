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
            //TODO: доделац
            await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                _vkManager.OnLoadProgress += _vkManager_OnLoadProgress;
                await _vkManager.DownloadVideosAsync(videos, videosAlbumTitle);

                while (!ctx.IsFinished)
                {
                    task1.Increment(1);
                }
            });

            
            Log.Information("Videos downloaded");
        }

        private void _vkManager_OnLoadProgress(int percent)
        {
        }
    }
}
