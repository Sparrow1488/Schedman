using Schedman.Entities;
using Schedman.Tools.IO;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Schedman.Example.Next
{
    internal class Program
    {
        private static async Task Main()
        {
            var access = new AccessPermission(ConfigurationManager.AppSettings["accessFile"]);
            var manager = new VkManager();
            await manager.AuthorizateAsync(access);

            var group = await manager.GetGroupManagerAsync("пердельня");
            Console.WriteLine($"GROUP => id:{group.Id}, title:{group.Title}");
            var publishes = await group.GetPublishesAsync(page: 1, count: 20);
            Console.WriteLine("Publishes count => " + publishes.Count());

            var imageSource = await group.UploadServer.UploadImageAsync(ConfigurationManager.AppSettings["imageFile"]);
            var publishEntity = new VkPublishEntity()
            {
                Message = "Hello world!"
            };
            publishEntity.MediaCollection.Add(imageSource);

            var result = await group.PublishAsync(publishEntity);
            if (result.AllSuccess)
            {
                Console.WriteLine("Все публикации успешно загружены");
            }
            else
            {
                var notUploaded = result.FailedToUpload;
                Console.WriteLine("Не удалось загрузить: " + notUploaded.Count());
                result.ThrowIfHasFails();
            }

            var videos = await manager.GetVideosFromOwnAlbumAsync("57");
            var firstVideo = videos.First().Files;
            var saveConfig = new SaveServiceConfiguration("56 - downloads", "./downloads");
            var saveService = new SaveService(saveConfig);
            var progress = new Progress<IntermediateProgressResult>(progress =>
                    Console.WriteLine($"Downloaded ({progress.CurrentPercentsStringify}%)"));

            foreach (var video in videos)
            {
                Console.WriteLine("DOWNLOAD STARTED => " + video.Title);
                var videoBytes = await manager.DownloadVideoAsync(video, progress);
                Console.WriteLine("SAVE => " + video.Title);
                await saveService.SaveLocalAsync(videoBytes, SaveFileInfo.Name(video.Title).Mp4());
            }

            Console.WriteLine("Tap to exit...");
            Console.ReadKey();
        }
    }
}
