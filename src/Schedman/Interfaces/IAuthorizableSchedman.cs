using Schedman.Entities;
using System.Threading.Tasks;

namespace Schedman.Interfaces
{
    public interface IAuthorizableSchedman
    {
        public bool IsAuthorizated { get; }
        Task AuthorizateAsync(AccessPermission accessPermission);
    }
}
