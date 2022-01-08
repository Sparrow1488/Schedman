using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class CreatePost
    {
        public long Id { get; internal set; }
        public string Message { get; set; }
        public IEnumerable<string> PhotosUrl { get; set; }
    }
}
