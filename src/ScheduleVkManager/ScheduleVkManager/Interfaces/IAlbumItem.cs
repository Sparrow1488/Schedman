using ScheduleVkManager.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Interfaces
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
