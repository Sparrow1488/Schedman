using Schedman.Tools.IO;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Services;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Schedman.Example.Next.Services
{
    public class DownloadVideosService : IService
    {
        public DownloadVideosService(VkManager manager)
        {
            Manager = manager;
        }

        public VkManager Manager { get; }

        public async Task StartAsync()
        {
            var albumName = ConfigurationManager.AppSettings["downloadAlbumName"];
            var videos = await Manager.GetVideosFromOwnAlbumAsync(albumName);
            var firstVideo = videos.First().Files;
            string downloadedAlbumName = $"{albumName}_downloads";
            var saveConfig = new SaveServiceConfiguration(downloadedAlbumName, "./downloads");
            var saveService = new SaveService(saveConfig);
            var progress = new Progress<IntermediateProgressResult>(progress =>
                    Console.WriteLine($"Downloaded ({progress.CurrentPercentsStringify}%)"));

            foreach (var video in videos)
            {
                var videoId = video.Id;
                if (File.Exists(Path.Combine(saveConfig.Root, downloadedAlbumName, video.Title + videoId + ".mp4")))
                {
                    Console.WriteLine($"File {video.Title} already exists. Continue");
                    continue;
                }

                try
                {
                    Console.WriteLine("DOWNLOAD STARTED => " + video.Title);
                    var videoBytes = await Manager.DownloadVideoAsync(video, progress);
                    Console.WriteLine("SAVE => " + video.Title);
                    await saveService.SaveLocalAsync(videoBytes, SaveFileInfo.Name(video.Title + videoId).Mp4());
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"\tEXCEPTION => {ex.GetType().Name}:{ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }

            Console.WriteLine("Tap to exit...");
            Console.ReadKey();
        }
    }
}
