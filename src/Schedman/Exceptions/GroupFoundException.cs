using System;

namespace Schedman.Exceptions
{
    public class GroupFoundException : Exception
    {
        public GroupFoundException(string message) : base(message)
        {
        }
    }
}
