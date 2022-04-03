using System;

namespace Schedman.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() { }
        public override string Message => "Authorization failed";
    }
}
