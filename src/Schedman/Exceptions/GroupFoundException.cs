using System;

namespace Schedman.Exceptions
{
    public class GroupFoundException : SchedmanException
    {
        public GroupFoundException(string message) : base(message) { }
    }
}
