using System;

namespace VkSchedman.Exceptions
{
    public class GroupFoundException : Exception
    {
        public GroupFoundException(string message) : base(message)
        {
        }
    }
}
