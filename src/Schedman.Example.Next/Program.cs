using Schedman.Entities;
using System;
using System.Collections.Generic;
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
            if (manager.IsAuthorizated)
            {
                var group = await manager.GetGroupManagerAsync("пердельня");
                Console.WriteLine($"GROUP => id:{group.Id}, title:{group.Title}");
                var publishes = await group.GetPublishesAsync();
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
            }
            else
            {
                Console.WriteLine("Authorization failed");
            }

            Console.WriteLine("Tap to exit...");
            Console.ReadKey();
        }
    }
}
