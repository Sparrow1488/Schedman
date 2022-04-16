using System.Collections.Generic;
using Schedman.Interfaces;

namespace Schedman.Entities
{
    public class AlbumItem : IAlbumItem
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Album Album { get; set; }
        public IEnumerable<Album> GetAlbums() => null;
        public IEnumerable<AlbumItem> GetItems() => new List<AlbumItem>() { this };
        public bool IsAlbum() => false;
        public bool IsItem() => true;
    }
}
