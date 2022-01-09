using ScheduleVkManager.Entities;
using ScheduleVkManager.Storage;
using ScheduleVkManager.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleVkManager
{
    public class Program
    {
        private static GroupManager _group;
        private static readonly Logger _logger = new Logger();
        private static readonly Scheduler _scheduler = new Scheduler();
        private static readonly FileStorage _storage = new FileStorage();
        private static readonly List<TimeSpan> _times = new List<TimeSpan>() {
                new TimeSpan(0, 0, 0),
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

            //CreatePostsUsingAlbums(_scheduler);
            //Console.WriteLine();
            //#region VkManager

            //Task.Run(async () =>
            //{
            //    var authDataPath = ConfigurationManager.AppSettings["auth"];
            //    var authData = new AuthorizeData(authDataPath);
            //    var vkManager = new VkManager();
            //    var authResult = await vkManager.AuthorizeAsync(authData);
            //    if (authResult is false)
            //    {
            //        PrintErrors(vkManager.Errors);
            //        vkManager.ClearErrors();
            //        throw new Exception("Auth error");
            //    }
            //    else _logger.Success("Authorize success");

            //    _group = await vkManager.GetGroupManagerAsync("full party");
            //    if (_group.Id == 0) {
            //        PrintErrors(vkManager.Errors);
            //        throw new Exception("Cannot find group");
            //    }
            //    else _logger.Success("Success found group id_" + _group.Id);

            //    _scheduler.Create(_times, 2, 150);
            //    var posts = CreatePostsUsingAlbums(_scheduler);
            //    //post = await _group.AddPostAsync(post);
            //    //if (post.Id == 0)
            //    //{
            //    //    _logger.Error("Cannot create post on wall");
            //    //}
            //    //else _logger.Success("Success created post on wall id_" + post.Id);
            //    //PrintPostInfo(post);

            //}).GetAwaiter().GetResult();
            //#endregion
        }

        

        //private static IEnumerable<CreatePost> CreatePostsUsingAlbums(IEnumerable<DateTime> scheduleTimes)
        //{
        //    var result = new List<CreatePost>();
        //    var mainAlbum = _albums.GetAlbum(ConfigurationManager.AppSettings["mainAlbumPath"]);
        //    foreach (var schedule in scheduleTimes) {
        //        foreach (var authorsAlbum in mainAlbum.Albums) {
        //            result.AddRange(CreateAuthorPosts(authorsAlbum, schedule));
        //        }
        //    }
        //    return result;
        //}

        //private static IEnumerable<CreatePost> CreateAuthorPosts(Album authorAlbum, DateTime? schedule = null)
        //{
        //    var result = new List<CreatePost>();
        //    foreach (var unitPhoto in authorAlbum.Items) {
        //        result.Add(CreatePost(new List<AlbumItem>() { unitPhoto }));
        //    }
        //    foreach (var chapterPhotos in authorAlbum.Albums) {
        //        CreatePost chapterPost;
        //        foreach (var chapter in chapterPhotos.Items) {

        //        }
        //    }

        //    return result;
        //}

        //private static CreatePost CreatePost(IEnumerable<AlbumItem> photos, string author)
        //{
        //    var post = new CreatePost();
        //    post.Message = "Author-" + author;
        //    var urls = new List<string>();
        //    foreach (var photo in photos) {
        //        urls.Add(Path.Combine(photo.Album.Path, photo.Name + photo.Extension));
        //    }
        //    post.PhotosUrl = urls;
        //    post.
        //}

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
