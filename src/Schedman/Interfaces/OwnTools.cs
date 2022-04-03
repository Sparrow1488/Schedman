using Schedman.Entities;
using System.Threading.Tasks;

namespace Schedman.Interfaces
{
    public abstract class OwnTools
    {
        public abstract Task<UserInfo> GetInfoAsync();
    }
}
