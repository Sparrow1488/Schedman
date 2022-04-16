namespace Schedman.Exceptions
{
    public class SchedmanPublishFailedException : SchedmanException
    {
        public SchedmanPublishFailedException() { }
        public SchedmanPublishFailedException(string message) : base(message) { }
    }
}
