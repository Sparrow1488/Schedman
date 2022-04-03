using System;

namespace Schedman.Exceptions
{
    public class AlbumNotFoundException : Exception
    {
        public AlbumNotFoundException(string message) : base(message) { }
    }
}
