using Schedman.Tools.IO.Abstractions;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Exceptions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Schedman.Tools.IO.Services
{
    public class SaveService : ISaveService
    {
        public SaveService(SaveServiceConfiguration config) =>
            _config = config;

        private readonly SaveServiceConfiguration _config;

        public void SaveLocal(byte[] bytes, SaveFileInfo saveInfo)
        {
            ThrowIfNullOrInvalid(saveInfo);
            var saveFileFullPath = PrepareDirectoryAndGetFileFullPath(saveInfo);
            File.WriteAllBytes(saveFileFullPath, bytes);
        }

        public async Task SaveLocalAsync(byte[] bytes, SaveFileInfo saveInfo)
        {
            ThrowIfNullOrInvalid(saveInfo);
            var saveFileFullPath = PrepareDirectoryAndGetFileFullPath(saveInfo);
            await File.WriteAllBytesAsync(saveFileFullPath, bytes);
        }

        private string PrepareDirectoryAndGetFileFullPath(SaveFileInfo saveInfo)
        {
            string validFileName = MakeFileNameValid(saveInfo.FileName);
            string dirFullPath = Path.Combine(_config.Root, _config.DirectoryName);
            Directory.CreateDirectory(dirFullPath);
            string saveFileFullPath = Path.Combine(dirFullPath, validFileName) + saveInfo.Extension;
            saveFileFullPath = MakeFileFullPathValidIfRepeatInDirectory(saveFileFullPath);
            return saveFileFullPath;
        }

        private void ThrowIfNullOrInvalid(SaveFileInfo saveInfo)
        {
            if (saveInfo == null || string.IsNullOrWhiteSpace(saveInfo?.FileName))
            {
                throw new InvalidSaveFileParamException();
            }
        }

        private string MakeFileNameValid(string fileName) =>
            fileName.Replace("\\", "").Replace("/", "");

        private string MakeFileFullPathValidIfRepeatInDirectory(string fileFullPath)
        {
            var directoryPath = Path.GetDirectoryName(fileFullPath);
            var fileName = Path.GetFileName(fileFullPath);
            var repeatedFilesInDirectory = Directory.GetFiles(directoryPath)
                                                    .Where(file => file.Contains(fileName))
                                                    .Count();
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileFullPath);
            fileNameWithoutExtension += repeatedFilesInDirectory > 0 ? $" ({repeatedFilesInDirectory})" : "";
            string fileExtension = Path.GetExtension(fileFullPath);
            return Path.Combine(directoryPath, fileNameWithoutExtension) + fileExtension;
        }
    }
}
