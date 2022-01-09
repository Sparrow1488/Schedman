using ScheduleVkManager.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ScheduleVkManager.Examples
{
    public class Program
    {
        public static void Main()
        {
            var posts = new List<CreatePost>();
            var albums = new AuthorsHandler();
            var authorsAlbum = albums.GetAlbum(ConfigurationManager.AppSettings["mainAlbumPath"]);
            foreach (var authoredAlbum in authorsAlbum.Items) {
                if(authoredAlbum is Album authorAlbum) {
                    var chapters = authorAlbum.GetAlbums();
                    if(chapters != null && chapters.Count() > 0)
                        chapters.ToList()
                                    .ForEach(chapter =>
                                        posts.Add(CreatePost(chapter.GetItems(), authorAlbum.Author)));
                    var unitPhotos = authorAlbum.GetItems()?.ToList();
                    if (unitPhotos != null && unitPhotos.Count() > 0)
                        unitPhotos.ForEach(unit => posts.Add(CreatePost(unit, authorAlbum.Author)));
                }
            }
        }

        private static CreatePost CreatePost(IEnumerable<AlbumItem> chapterItems, string author)
        {
            var result = new CreatePost();
            var urls = new List<string>();
            foreach (var chapterItem in chapterItems) {
                var photo = chapterItem.GetItems().First();
                urls.Add(Path.Combine(photo.Album.Path, photo.Name + photo.Extension));
            }
            result.Message = string.IsNullOrWhiteSpace(author) ? "" : "Author-" + author;
            result.PhotosUrl = urls;
            return result;
        }

        private static CreatePost CreatePost(AlbumItem unitItem, string author)
        {
            var result = new CreatePost();
            result.Message = string.IsNullOrWhiteSpace(author) ? "" : "Author-" + author;
            string path = Path.Combine(unitItem.Album.Path, unitItem.Name + unitItem.Extension);
            result.PhotosUrl = new List<string>() { path };
            return result;
        }
    }
}
