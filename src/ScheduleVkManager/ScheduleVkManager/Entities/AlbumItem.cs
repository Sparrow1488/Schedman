
using Newtonsoft.Json;

namespace ScheduleVkManager.Entities
{
    public class AlbumItem
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        [JsonIgnore]
        public Album Album { get; set; }
        public UploadStatus UploadStatus { get; set; }
    }
}
