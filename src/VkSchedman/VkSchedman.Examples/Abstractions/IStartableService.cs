using System.Threading.Tasks;

namespace VkSchedman.Examples.Abstractions
{
    internal interface IStartableService
    {
        Task StartAsync();
    }
}
