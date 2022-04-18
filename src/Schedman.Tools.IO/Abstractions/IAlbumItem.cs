using System.Collections.Generic;

namespace Schedman.Tools.IO.Interfaces
{
    public interface IAlbumItem
    {
        public string Name { get; set; }
        public IEnumerable<AlbumItem> GetItems();
        public IEnumerable<Album> GetAlbums();
        public bool IsItem();
        public bool IsAlbum();
    }
}
