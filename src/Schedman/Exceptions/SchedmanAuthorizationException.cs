using System;

namespace Schedman.Exceptions
{
    public class SchedmanAuthorizationException : Exception
    {
        public SchedmanAuthorizationException() { }
        public override string Message => "Authorization failed";
    }
}
