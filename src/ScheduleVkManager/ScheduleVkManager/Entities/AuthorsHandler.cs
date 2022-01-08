using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleVkManager.Entities
{
    public class AuthorsHandler : IAlbumsHandler<AlbumItem>
    {
        public Album GetAlbum(string albumPath)
        {
            var result = CreateAlbum(albumPath);
            result = UpdateFiles(result);
            var albums = new List<Album>();
            foreach (var album in Directory.GetDirectories(albumPath)) {
                albums.Add(GetAlbum(album));
            }
            result.Albums = albums;
            return result;
        }

        private Album CreateAlbum(string albumPath)
        {
            var dirInfo = new DirectoryInfo(albumPath);
            var result = new Album() {
                Author = dirInfo.Name.ToLower()?.Replace("author-", "")?.Trim() ?? "unknown",
                Path = albumPath
            };
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
            album.Items = albumItems;
            return album;
        }

        /// <summary>
        /// В AuthorsHandler е по умолчанию используется паттерн для поиска AlbumItems и Album - "author-". Его можно переназначить
        /// </summary>
        public void SetPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        private UploadStatus CheckUploadedStatus(Album album)
        {
            // TODO: interact with storage or db
            throw new NotImplementedException();
        }

        private UploadStatus CheckUploadedStatus(AlbumItem albumItem)
        {
            // TODO: interact with storage or db
            throw new NotImplementedException();
        }
    }
}
