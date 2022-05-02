namespace Schedman.Exceptions
{
    internal class SchedmanNoGroupAccess : SchedmanException
    {
        public SchedmanNoGroupAccess() { }
        public SchedmanNoGroupAccess(string message) : base(message) { }
    }
}
