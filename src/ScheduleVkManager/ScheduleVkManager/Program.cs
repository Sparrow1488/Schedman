using ScheduleVkManager.Entities;
using ScheduleVkManager.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScheduleVkManager
{
    public class Program
    {
        private static Logger _logger = new Logger();
        public static void Main()
        {
            _logger.Log("Started SheduleVkManager");
            Task.Run(async () =>
            {
                var authDataPath = @"C:\Users\Александр\Downloads\need_delete\delete_pas_log.txt";
                var authData = new AuthorizeData(authDataPath);
                var vkManager = new VkManager();
                var authResult = await vkManager.AuthorizeAsync(authData);
                if(!(authResult is true)) {
                    PrintErrors(vkManager.Errors);
                    vkManager.ClearErrors();
                }
                else _logger.Success("Authorize success");

                var group = await vkManager.GetGroupManagerAsync("full party");
                if(group.Id == 0) {
                    PrintErrors(vkManager.Errors);
                }
                else _logger.Success("Success found group id_" + group.Id);

                var post = CreatePost();
                post = await group.AddPostAsync(post);
                if(post.Id == 0) {
                    _logger.Error("Cannot create post on wall");
                }
                else _logger.Success("Success created post on wall id_" + post.Id);
            }).GetAwaiter().GetResult();
        }

        private static IEnumerable<string> GetRandomPhotosUrl()
        {
            var result = new List<string>();
            string rootDir = @"C:\Users\Александр\Downloads\author-aestheticc_meme";
            var filesInDir = Directory.GetFiles(rootDir);
            var rnd = new Random();
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);

            return result;
        }

        private static CreatePost CreatePost()
        {
            var result = new CreatePost();
            _logger.Log("Let's create new post on wall!");
            result.Message = _logger.Input("Message");
            var photos = _logger.Input("Photos url (\"rnd\"if upload random)");
            if(photos == "rnd")
                result.PhotosUrl = GetRandomPhotosUrl();
            return result;
        }

        private static void PrintErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors) {
                _logger.Error(error);
            }
        }
    }
}
