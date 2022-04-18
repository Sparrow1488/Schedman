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
            
            //var group = await manager.GetGroupManagerAsync("пердельня");
            //Console.WriteLine($"GROUP => id:{group.Id}, title:{group.Title}");
            //var publishes = await group.GetPublishesAsync();
            //Console.WriteLine("Publishes count => " + publishes.Count());

            //var imageSource = await group.UploadServer.UploadImageAsync(ConfigurationManager.AppSettings["imageFile"]);
            //var publishEntity = new VkPublishEntity()
            //{
            //    Message = "Hello world!"
            //};
            //publishEntity.MediaCollection.Add(imageSource);

            //var result = await group.PublishAsync(publishEntity);
            //if (result.AllSuccess)
            //{
            //    Console.WriteLine("Все публикации успешно загружены");
            //}
            //else
            //{
            //    var notUploaded = result.FailedToUpload;
            //    Console.WriteLine("Не удалось загрузить: " + notUploaded.Count());
            //    result.ThrowIfHasFails();
            //}

            var videos = await manager.GetVideosFromOwnAlbumAsync("54");
            var saveConfig = new SaveServiceConfiguration("54 - downloads", "./downloads");
            var saveService = new SaveService(saveConfig);
            foreach (var video in videos)
            {
                var videoBytes = await manager.DownloadVideoAsync(video);
                await saveService.SaveLocalAsync(videoBytes, SaveFileInfo.Name(video.Title).Mp4());
                Console.WriteLine("SAVE => " + video.Title);
            }

            Console.WriteLine("Tap to exit...");
            Console.ReadKey();
        }
    }
}
