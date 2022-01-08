using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public interface IAlbumsHandler<T>
        where T : AlbumItem
    {
        public bool SetPattern(string key, string pattern);
        public IDictionary<string, string> GetPatterns();
        public Album GetAlbum(string albumPath);
    }
}
