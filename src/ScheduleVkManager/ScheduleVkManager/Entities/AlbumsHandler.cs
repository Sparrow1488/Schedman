using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public interface IAlbumsHandler<T>
        where T : AlbumItem
    {
        public void SetPattern(string pattern);
        public Album GetAlbum(string albumPath);
    }
}
