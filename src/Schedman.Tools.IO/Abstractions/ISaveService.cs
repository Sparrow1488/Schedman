using System.Threading.Tasks;

namespace Schedman.Tools.IO.Abstractions
{
    public interface ISaveService
    {
        Task SaveLocalAsync(byte[] bytes, SaveFileInfo saveInfo);
        void SaveLocal(byte[] bytes, SaveFileInfo saveInfo);
    }
}
