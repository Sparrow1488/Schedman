using System;

namespace VkSchedman.Exceptions
{
    public class InvalidInputDateException : Exception
    {
        public InvalidInputDateException(string message) : base(message)
        {
        }
    }
}
