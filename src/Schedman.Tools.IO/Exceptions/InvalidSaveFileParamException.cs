using System;

namespace Schedman.Tools.IO.Exceptions
{
    public class InvalidSaveFileParamException : Exception
    {
        public InvalidSaveFileParamException() { }
        public InvalidSaveFileParamException(string message) : base(message) {}
    }
}