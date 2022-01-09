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
    internal class Program
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

        public static void Main()
        {
            _logger.Log("Started SheduleVkManager");

            Task.Run(async () =>
            {
                _logger.Log("Try authorize...");
                var authDataPath = ConfigurationManager.AppSettings["auth"];
                var authData = new AuthorizeData(authDataPath);
                var vkManager = new VkManager();
                var authResult = await vkManager.AuthorizeAsync(authData);
                if (authResult is false) {
                    PrintErrors(vkManager.Errors);
                    vkManager.ClearErrors();
                    throw new Exception("Auth error");
                }
                else _logger.Success("Authorize success");


                string findGroupName = "Full party";
                _logger.Log($"Get group named \"{findGroupName}\"");
                _group = await vkManager.GetGroupManagerAsync(findGroupName);
                if (_group.Id == 0) {
                    PrintErrors(vkManager.Errors);
                    throw new Exception("Cannot find group");
                }
                else _logger.Success("Success found group, id_" + _group.Id);


                _logger.Log("Starting create posts...");
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

        private static void PrintErrors(IEnumerable<string> errors) =>
            errors.ToList().ForEach(error => _logger.Error(error));
    }
}
