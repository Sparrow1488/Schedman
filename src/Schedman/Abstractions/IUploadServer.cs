using Schedman.Entities;
using System.Threading.Tasks;

namespace Schedman.Abstractions
{
    public interface IUploadServer
    {
        Task<WebImage> UploadImageAsync(string filePath);
    }
}
