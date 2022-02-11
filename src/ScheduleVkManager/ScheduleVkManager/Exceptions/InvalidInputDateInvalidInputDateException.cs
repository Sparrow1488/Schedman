using System;

namespace ScheduleVkManager.Exceptions
{
    public class InvalidInputDateException : Exception
    {
        public InvalidInputDateException(string message) : base(message)
        {
        }
    }
}
