using System;

namespace Schedman.Exceptions
{
    public class SchedmanException : Exception
    {
        public SchedmanException() { }
        public SchedmanException(string message) : base(message) { }
    }
}
