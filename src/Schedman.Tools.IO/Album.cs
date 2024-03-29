﻿using Schedman.Tools.IO.Interfaces;
using System.Collections.Generic;

namespace Schedman.Tools.IO
{
    public class Album : IAlbumItem
    {
        public string Author { get; set; } = "unknown";
        public string Chapter { get; set; } = "unknown";
        public string Path { get; set; }
        public IEnumerable<IAlbumItem> Items { get; internal set; }
        public string Name { get; set; }

        public IEnumerable<Album> GetAlbums()
        {
            var results = new List<Album>();
            foreach (var item in Items)
            {
                if (item.IsAlbum())
                    results.Add(item as Album);
            }
            return results;
        }

        public IEnumerable<AlbumItem> GetItems() 
        {
            var results = new List<AlbumItem>();
            foreach (var item in Items)
            {
                if (item.IsItem())
                    results.Add(item as AlbumItem);
            }
            return results;
        }

        public bool IsAlbum() => true;

        public bool IsItem() => false;
    }
}
