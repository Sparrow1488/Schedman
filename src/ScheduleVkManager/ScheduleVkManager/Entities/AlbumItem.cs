using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class AlbumItem
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Album Album { get; set; }
        public UploadStatus UploadStatus { get; set; }
    }
}
