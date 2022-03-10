using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Entities;
using VkSchedman.Examples.Abstractions;
using VkSchedman.Extensions;
using VkSchedman.Tools;

namespace VkSchedman.Examples.Services
{
    internal sealed class VkSchedulerService : IStartableService
    {
        public VkSchedulerService(VkManager vkManager)
        {
            vkManager.ThrowIfNotAuth();
            _vkManager = vkManager;
        }

        private GroupManager _group;
        private VkManager _vkManager;
        private readonly Scheduler _scheduler = new Scheduler();
        private readonly PostEditor _postEditor = new PostEditor();
        private readonly PublicationsLogger _postLogger = new PublicationsLogger();
        private readonly List<TimeSpan> _times = CreateTimes();

        public async Task StartAsync()
        {
            _group = await _vkManager.GetGroupManagerAsync("full party");
            await StartScheduleVkManagerAsync();
        }

        private async Task StartScheduleVkManagerAsync()
        {
            string findGroupName = "Full party";
            Log.Information($"Get group named \"{findGroupName}\"");

            Log.Information("Starting create posts...");
            var posts = _postEditor.CreatePostRange();
            _scheduler.Create(_times, 30, posts.Count(), startTime: new DateTime(2022, 3, 11, 21, 0, 0));
            posts = posts.Shuffle();
            posts = _postEditor.SetSchedule(posts, _scheduler);

            int postCount = posts.Count();
            int currentPostNum = 0;
            foreach (var post in posts)
            {
                try
                {
                    var createdPost = await _group.AddPostAsync(post);
                    Log.Information($"({currentPostNum + 1}|{postCount}) Post was success loaded");
                }
                catch (PostLimitException e)
                {
                    Log.Error(e.Message);
                    _postLogger.LogNotPublicated(post);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, ex.Message);
                    _postLogger.LogNotPublicated(post);
                }
                finally
                {
                    currentPostNum++;
                }
            }
        }

        #region CreateTimesRegion

        private static List<TimeSpan> CreateTimes() =>
            new List<TimeSpan>() {
                new TimeSpan(0, 0, 0),
                new TimeSpan(5, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(17, 0, 0),
                new TimeSpan(19, 0, 0),
                new TimeSpan(23, 0, 0)
            };

        #endregion
    }
}
