using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Schedman.Example.Next.Services
{
    public interface IService
    {
        IConfiguration Configuration { get; set; }
        Task StartAsync();
    }
}
