using System;

namespace Schedman.Exceptions
{
    public class SchedmanGroupNotFoundException : SchedmanException
    {
        public SchedmanGroupNotFoundException(string message) : base(message) { }
    }
}
