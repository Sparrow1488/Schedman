using Schedman.Entities;
using System.Collections.Generic;

namespace Schedman.Interfaces
{
    public interface IAlbumsHandler<T>
        where T : AlbumItem
    {
        public bool SetPattern(string key, string pattern);
        public IDictionary<string, string> GetPatterns();
        public Album GetAlbum(string albumPath);
    }
}
