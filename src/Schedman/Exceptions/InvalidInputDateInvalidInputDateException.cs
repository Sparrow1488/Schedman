using System;

namespace Schedman.Exceptions
{
    public class InvalidInputDateException : Exception
    {
        public InvalidInputDateException(string message) : base(message)
        {
        }
    }
}
