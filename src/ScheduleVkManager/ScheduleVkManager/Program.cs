using Microsoft.Extensions.Configuration;
using ScheduleVkManager.Entities;
using ScheduleVkManager.Extensions;
using ScheduleVkManager.Storage;
using Serilog;
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
            var builder = new ConfigurationBuilder();
            InitConfiguration(builder);

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .ReadFrom.Configuration(builder.Build())
                         .WriteTo.Console()
                         .WriteTo.File("logging.txt")
                         .CreateLogger();

            Log.Information("Started SheduleVkManager");


            Task.Run(async () =>
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
                    throw new Exception("Auth error");
                }
                else Log.Information("Authorize success");


                string findGroupName = "Full party";
                Log.Information($"Get group named \"{findGroupName}\"");
                _group = await vkManager.GetGroupManagerAsync(findGroupName);
                if (_group.Id == 0)
                {
                    vkManager.Errors.PrintErrors();
                    throw new Exception("Cannot find group");
                }
                else Log.Information("Success found group, id_" + _group.Id);


                Log.Information("Starting create posts...");
                var posts = _postEditor.CreatePostRange();
                _scheduler.Create(_times, 30, posts.Count(), new DateTime(2022, 01, 22));
                posts = posts.Shuffle();
                posts = _postEditor.SetSchedule(posts, _scheduler);
                foreach (var post in posts)
                {
                    try
                    {
                        var createdPost = await _group.AddPostAsync(post);
                        Log.Information("Post was success loaded");
                    }
                    catch (PostLimitException e)
                    {
                        Log.Error(e.Message);
                        using (var swFile = new StreamWriter(File.OpenWrite("./unloaded-errors.txt")))
                        {
                            swFile.WriteLine($"@\nmessage:{post.Message}");
                            foreach (var photo in post.PhotosUrl)
                                swFile.WriteLine(photo);
                            swFile.WriteLine("@");
                        }
                    }
                }

            }).GetAwaiter().GetResult();

            Log.CloseAndFlush();
        }

        private static void InitConfiguration(IConfigurationBuilder builder) =>
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    }
}
