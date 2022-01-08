using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class Album
    {
        public string Author { get; set; } = "unknown";
        public string Chapter { get; set; } = "unknown";
        public string Path { get; internal set; }
        public Album Parent { get; set; }
        public UploadStatus UploadStatus { get; internal set; }
        public IEnumerable<AlbumItem> Items { get; internal set; }
        public IEnumerable<Album> Albums { get; internal set; }
    }
}
