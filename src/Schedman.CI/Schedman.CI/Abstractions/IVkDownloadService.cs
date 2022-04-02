using System.Threading.Tasks;

namespace Schedman.CI.Abstractions
{
    internal interface IVkDownloadService
    {
        Task DownloadAlbumVideosAsync(string albumTitle);
    }
}
