using System;

namespace Schedman.Exceptions
{
    public class AlbumNotFoundException : SchedmanException
    {
        public AlbumNotFoundException(string message) : base(message) { }
    }
}
