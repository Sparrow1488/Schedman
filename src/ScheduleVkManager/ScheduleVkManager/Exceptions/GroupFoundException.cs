using System;

namespace ScheduleVkManager.Exceptions
{
    public class GroupFoundException : Exception
    {
        public GroupFoundException(string message) : base(message)
        {
        }
    }
}
