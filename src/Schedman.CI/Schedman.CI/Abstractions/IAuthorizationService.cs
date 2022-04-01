using System.Threading.Tasks;

namespace Schedman.CI.Abstractions
{
    internal interface IAuthorizationService
    {
        Task AuthorizeAsync();
    }
}
