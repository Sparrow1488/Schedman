using Schedman.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Schedman.Entities
{
    public class AuthorsHandler : IAlbumsHandler<AlbumItem>
    {
        private IDictionary<string, string> _patterns { get; set; } = new Dictionary<string, string>();

        public AuthorsHandler()
        {
            _patterns.Add("author", "author-");
            _patterns.Add("chapter", "chapter-");
        }

        public Album GetAlbum(string mainPath)
        {
            var result = CreateAlbum(mainPath);
            result = UpdateFiles(result);
            var albums = new List<Album>();
            foreach (var album in Directory.GetDirectories(mainPath)) {
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
            var result = new Album() {
                Path = albumPath,
                //Parent = _parentAlbum
            };
            string albumName = dirInfo.Name.ToLower();
            if (albumName.Contains(_patterns["author"])) {
                result.Author = albumName?.Replace(_patterns["author"], "")?.Trim() ?? result.Author;
            }
            else if (albumName.Contains(_patterns["chapter"]) || !albumName.Contains(_patterns["author"])) {
                result.Chapter = albumName?.Replace(_patterns["chapter"], "")?.Trim() ?? result.Author;
            }
            //_parentAlbum = result;
            //result.UploadStatus = CheckUploadedStatus(result);
            return result;
        }

        private Album UpdateFiles(Album album)
        {
            var albumItems = new List<AlbumItem>();
            var files = Directory.GetFiles(album.Path);
            foreach (var file in files) {
                var fileInfo = new FileInfo(file);
                albumItems.Add(new AlbumItem() {
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

        /// <summary>
        /// В AuthorsHandler е по умолчанию используется паттерн для поиска AlbumItems и Album - "author-". Его можно переназначить
        /// </summary>
        public bool SetPattern(string key, string pattern) =>
            _patterns.TryAdd(key, pattern);

        public IDictionary<string, string> GetPatterns() => _patterns;

        private UploadStatus CheckUploadedStatus(Album album) => 
            throw new NotImplementedException();

        private UploadStatus CheckUploadedStatus(AlbumItem albumItem) => 
            throw new NotImplementedException();

    }
}
