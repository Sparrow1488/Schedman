using System.Threading.Tasks;

namespace Schedman.Abstractions
{
    internal abstract class CommandBase
    {
        public abstract Task ExecuteAsync();
        public abstract TResult GetResultOrDefault<TResult>();
    }
}
