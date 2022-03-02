using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Entities;
using VkSchedman.Exceptions;
using VkSchedman.Extensions;
using VkSchedman.Tools;

namespace VkSchedman
{
    public sealed class Program
    {
        private static GroupManager _group;
        private static readonly Scheduler _scheduler = new Scheduler();
        private static readonly PostEditor _postEditor = new PostEditor();
        private static readonly PublicationsLogger _postLogger = new PublicationsLogger();
        private static readonly List<TimeSpan> _times = CreateTimes();

        public static void Main()
        {
            var builder = new ConfigurationBuilder();
            InitConfiguration(builder);

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .ReadFrom.Configuration(builder.Build())
                         .WriteTo.Console()
                         .WriteTo.File("logging.txt")
                         .CreateLogger();

            Log.Information("Started SheduleVkManager");

            try
            {
                //Task.Run(async () => await StartScheduleVkManagerAsync()).GetAwaiter().GetResult();
                Task.Run(async () => await ProcessVideosAsync()).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task ProcessVideosAsync()
        {
            var authDataPath = System.Configuration.ConfigurationManager.AppSettings["Auth"];
            var authData = new AuthorizeData(authDataPath);
            var vkManager = new VkManager();
            Log.Information("Try authorize...");
            if (await vkManager.AuthorizeAsync(authData))
            {
                Log.Information("Authorize success");
                string videosAlbumTitle = "52";
                var videos = await vkManager.GetVideosFromAlbumAsync(videosAlbumTitle, count:1000);
                await vkManager.DownloadVideosAsync(videos, videosAlbumTitle);
            }
            else throw new VkAuthorizationException("Authorize failed");
        }

        private static async Task StartScheduleVkManagerAsync()
        {
            Log.Information("Try authorize...");
            var authDataPath = System.Configuration.ConfigurationManager.AppSettings["Auth"];
            var authData = new AuthorizeData(authDataPath);
            var vkManager = new VkManager();
            var authResult = await vkManager.AuthorizeAsync(authData);
            if (authResult is false)
            {
                vkManager.Errors.PrintErrors();
                vkManager.ClearErrors();
                throw new VkAuthorizationException("Auth error");
            }
            else Log.Information("Authorize success");


            string findGroupName = "Full party";
            Log.Information($"Get group named \"{findGroupName}\"");
            _group = await vkManager.GetGroupManagerAsync(findGroupName);
            if (_group.Id == 0)
            {
                vkManager.Errors.PrintErrors();
                throw new GroupFoundException("Cannot find group");
            }
            else Log.Information("Success found group, id_" + _group.Id);


            Log.Information("Starting create posts...");
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
                    Log.Information($"({currentPostNum}|{postCount}) Post was success loaded");
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

        private static void InitConfiguration(IConfigurationBuilder builder) =>
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        #region CreateTimesRegion

        private static List<TimeSpan> CreateTimes() =>
            new List<TimeSpan>() {
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

        #endregion

    }
}
