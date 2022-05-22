using Schedman.Tools.IO.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Schedman.Tools.IO.Abstractions
{
    public interface IFileSystem
    {
        IEnumerable<Album> GetAlbums(string path);
    }

    public class FileSystem : IFileSystem
    {
        public IEnumerable<Album> GetAlbums(string path)
        {
            var albums = new List<Album>();
            var dirs = Directory.GetDirectories(path);
            foreach (var item in dirs)
            {
                albums.Add(GetAlbum(item));
            }
            return albums;
        }

        public Album GetAlbum(string mainPath)
        {
            var result = CreateAlbum(mainPath);
            result = UpdateFiles(result);
            var albums = new List<Album>();
            foreach (var album in Directory.GetDirectories(mainPath))
            {
                albums.Add(GetAlbum(album));
            }
            var itemsList = result.Items.ToList();
            itemsList.AddRange(albums);
            result.Items = itemsList;

            return result;
        }

        private Album CreateAlbum(string albumPath)
        {
            var dirInfo = new DirectoryInfo(albumPath);
            var result = new Album()
            {
                Path = albumPath,
            };
            //string albumName = dirInfo.Name.ToLower();
            result.Name = dirInfo.Name;
            //if (albumName.Contains(_patterns["author"]))
            //{
            //    result.Author = albumName?.Replace(_patterns["author"], "")?.Trim() ?? result.Author;
            //}
            //else if (albumName.Contains(_patterns["chapter"]) || !albumName.Contains(_patterns["author"]))
            //{
            //    result.Chapter = albumName?.Replace(_patterns["chapter"], "")?.Trim() ?? result.Author;
            //}
            return result;
        }

        private Album UpdateFiles(Album album)
        {
            var albumItems = new List<AlbumItem>();
            var files = Directory.GetFiles(album.Path);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                albumItems.Add(new AlbumItem()
                {
                    Name = fileInfo.Name,
                    Extension = fileInfo.Extension,
                    Album = album
                });
            }
            var itemsList = album.Items?.ToList() ?? new List<IAlbumItem>();
            itemsList.AddRange(albumItems);
            album.Items = itemsList;
            return album;
        }
    }
}
