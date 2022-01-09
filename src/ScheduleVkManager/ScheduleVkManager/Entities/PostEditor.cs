using ScheduleVkManager.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class PostEditor
    {
        /// <param name="cpecialPath">Get main directory path from App.config where key named "mainAlbumPath"</param>
        public IEnumerable<CreatePost> CreatePostRange(string cpecialPath = null)
        {
            var posts = new List<CreatePost>();
            var albums = new AuthorsHandler();
            var authorsAlbum = albums.GetAlbum(cpecialPath ?? 
                                                ConfigurationManager.AppSettings["mainAlbumPath"]);
            foreach (var authoredAlbum in authorsAlbum.Items) {
                if (authoredAlbum is Album authorAlbum) {
                    var chapters = authorAlbum.GetAlbums();
                    if (chapters != null && chapters.Count() > 0)
                        chapters.ToList()
                                    .ForEach(chapter =>
                                        posts.Add(CreatePost(chapter.GetItems(), authorAlbum.Author)));
                    var unitPhotos = authorAlbum.GetItems()?.ToList();
                    if (unitPhotos != null && unitPhotos.Count() > 0)
                        unitPhotos.ForEach(unit => posts.Add(CreatePost(unit, authorAlbum.Author)));
                }
            }
            return posts;
        }

        /// <summary>
        /// Set a schedule for posts. Note: Throw exception if count of posts && schedulers are difference. But not throw if count of schedulers more than posts
        /// </summary>
        public IEnumerable<CreatePost> SetSchedule(IEnumerable<CreatePost> posts, Scheduler scheduler)
        {
            var postsArr = posts.ToArray();
            var schedulerArr = scheduler.ToArray();
            for (int i = 0; i < postsArr.Length; i++) {
                postsArr[i].Schedule = schedulerArr[i];
            }
            return postsArr;
        }

        private CreatePost CreatePost(IEnumerable<AlbumItem> chapterItems, string author)
        {
            var result = new CreatePost();
            var urls = new List<string>();
            foreach (var chapterItem in chapterItems)
            {
                var photo = chapterItem.GetItems().First();
                urls.Add(Path.Combine(photo.Album.Path, photo.Name));
            }
            result.Message = string.IsNullOrWhiteSpace(author) ? "" : "Author-" + author;
            result.PhotosUrl = urls;
            return result;
        }

        private CreatePost CreatePost(AlbumItem unitItem, string author)
        {
            var result = new CreatePost();
            result.Message = string.IsNullOrWhiteSpace(author) ? "" : "Author-" + author;
            string path = Path.Combine(unitItem.Album.Path, unitItem.Name);
            result.PhotosUrl = new List<string>() { path };
            return result;
        }
    }
}
