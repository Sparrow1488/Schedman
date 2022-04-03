using System;
using System.Collections.Generic;

namespace VkSchedman.Entities
{
    public class CreatePost
    {
        public long Id { get; internal set; }
        public string Message { get; set; }
        public IEnumerable<string> PhotosUrl { get; set; }
        public DateTime? Schedule { get; set; }
    }
}
