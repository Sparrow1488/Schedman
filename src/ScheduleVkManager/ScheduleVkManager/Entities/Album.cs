using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScheduleVkManager.Entities
{
    public class Album
    {
        public string Author { get; set; } = "unknown";
        public string Chapter { get; set; } = "unknown";
        public string Path { get; set; }
        public Album Parent { get; set; }
        public UploadStatus UploadStatus { get; set; }
        public IEnumerable<AlbumItem> Items { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }
}
