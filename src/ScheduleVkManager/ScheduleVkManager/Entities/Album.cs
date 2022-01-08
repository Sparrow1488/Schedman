using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class Album
    {
        public string Author { get; set; }
        public string Chapter { get; set; }
        public string Path { get; internal set; }
        public UploadStatus UploadStatus { get; internal set; }
        public IEnumerable<AlbumItem> Items { get; internal set; }
        public IEnumerable<Album> Albums { get; internal set; }
    }
}
