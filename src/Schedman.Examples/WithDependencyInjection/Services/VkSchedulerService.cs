using Schedman;
using Schedman.Entities;
using Schedman.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Examples.Abstractions;

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
        private readonly VkManager _vkManager;
        private readonly Scheduler _scheduler = new Scheduler();
        private readonly PostEditor _postEditor = new PostEditor();
        private readonly List<TimeSpan> _times = CreateTimes();

        public async Task StartAsync()
        {
            _group = await _vkManager.GetGroupManagerAsync("Full party");
            await StartScheduleVkManagerAsync();
        }

        private async Task StartScheduleVkManagerAsync()
        {
            var posts = _postEditor.CreatePostRange();
            _scheduler.Create(_times, 30, posts.Count());
            posts = posts.Shuffle();
            posts = _postEditor.SetSchedule(posts, _scheduler);

            int postCount = posts.Count();
            int currentPostNum = 0;
            foreach (var post in posts)
            {
                try
                {
                    var createdPost = await _group.AddPostAsync(post);
                }
                catch (PostLimitException ex)
                {
                }
                catch (Exception ex)
                {
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
