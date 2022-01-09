using ScheduleVkManager.Entities;
using ScheduleVkManager.Extensions;
using ScheduleVkManager.Storage;
using ScheduleVkManager.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Exception;

namespace ScheduleVkManager
{
    public class Program
    {
        private static GroupManager _group;
        private static readonly Logger _logger = new Logger();
        private static readonly Scheduler _scheduler = new Scheduler();
        private static readonly PostEditor _postEditor = new PostEditor();
        private static readonly FileStorage _storage = new FileStorage();
        private static readonly List<TimeSpan> _times = new List<TimeSpan>() {
                new TimeSpan(0, 0, 0),
                new TimeSpan(3, 0, 0),
                new TimeSpan(5, 0, 0),
                new TimeSpan(7, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(17, 0, 0),
                new TimeSpan(19, 0, 0),
                new TimeSpan(21, 0, 0),
                new TimeSpan(23, 0, 0)
            };
        private static readonly IAlbumsHandler<AlbumItem> _albums = new AuthorsHandler();

        public static void Main()
        {
            _logger.Log("Started SheduleVkManager");

            Task.Run(async () =>
            {
                var authDataPath = ConfigurationManager.AppSettings["auth"];
                var authData = new AuthorizeData(authDataPath);
                var vkManager = new VkManager();
                var authResult = await vkManager.AuthorizeAsync(authData);
                if (authResult is false)
                {
                    PrintErrors(vkManager.Errors);
                    vkManager.ClearErrors();
                    throw new Exception("Auth error");
                }
                else _logger.Success("Authorize success");


                _group = await vkManager.GetGroupManagerAsync("Full party");
                if (_group.Id == 0)
                {
                    PrintErrors(vkManager.Errors);
                    throw new Exception("Cannot find group");
                }
                else _logger.Success("Success found group id_" + _group.Id);


                var posts = _postEditor.CreatePostRange();
                _scheduler.Create(_times, 30, posts.Count());
                posts = posts.Shuffle();
                posts = _postEditor.SetSchedule(posts, _scheduler);
                foreach (var post in posts) {
                    try {
                        var createdPost = await _group.AddPostAsync(post);
                        _logger.Success("Post was success loaded");
                    } catch (PostLimitException e) {
                        _logger.Error(e.Message);
                    }
                }

            }).GetAwaiter().GetResult();
        }

        private static void PrintPostInfo(CreatePost post)
        {
            _logger.Log($"[id_{post.Id}] Post info\nMessage: {post.Message}\nPhotos: {post.PhotosUrl.Count()}\nSchedule: {post.Schedule}");
        }

        private static void PrintErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors) {
                _logger.Error(error);
            }
        }

        private static IEnumerable<string> GetRandomPhotosUrl()
        {
            var result = new List<string>();
            string mainDirectory = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\Testable";
            var filesInDir = Directory.GetFiles(mainDirectory);
            var rnd = new Random();
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);

            return result;
        }
    }
}
