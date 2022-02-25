using System.Collections.Generic;
using VkSchedman.Interfaces;

namespace VkSchedman.Entities
{
    public class AlbumItem : IAlbumItem
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        //[JsonIgnore]
        public Album Album { get; set; }
        public UploadStatus UploadStatus { get; set; }
        public IEnumerable<Album> GetAlbums() => null;
        public IEnumerable<AlbumItem> GetItems() => new List<AlbumItem>() { this };
        public bool IsAlbum() => false;
        public bool IsItem() => true;
    }
}
