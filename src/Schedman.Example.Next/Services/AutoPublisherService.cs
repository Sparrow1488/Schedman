using Schedman.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Schedman.Example.Next.Services
{
    public class AutoPublisherService : IService
    {
        public AutoPublisherService(VkManager manager)
        {
            Manager = manager;
        }

        public VkManager Manager { get; }

        public IConfiguration Configuration { get; set; }

        public async Task StartAsync()
        {
            var group = await Manager.GetGroupManagerAsync("<group_name>");
            Console.WriteLine($"GROUP => id:{group.Id}, title:{group.Title}");
            var publishes = await group.GetPublishesAsync(page: 1, count: 20);
            Console.WriteLine("Publishes count => " + publishes.Count());

            const string noFile = "";
            var imageSource = await group.UploadServer.UploadImageAsync(noFile);
            var publishEntity = new VkPublishEntity()
            {
                Message = "Hello world!",
                CreatedAt = DateTime.Now + TimeSpan.FromMinutes(1)
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
    }
}
