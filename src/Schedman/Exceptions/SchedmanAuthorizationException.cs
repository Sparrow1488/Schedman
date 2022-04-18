namespace Schedman.Exceptions
{
    public class SchedmanAuthorizationException : SchedmanException
    {
        public SchedmanAuthorizationException() { }
        public override string Message => "Authorization failed";
    }
}
