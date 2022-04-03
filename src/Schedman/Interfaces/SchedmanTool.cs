using System.Threading.Tasks;
using Schedman.Exceptions;

namespace Schedman.Interfaces
{
    public abstract class SchedmanTool
    {
        public virtual bool IsAuthorized { get; protected set; }

        public abstract Task AuthorizeAsync();
        public abstract OwnTools GetOwnTools();
    }
}
