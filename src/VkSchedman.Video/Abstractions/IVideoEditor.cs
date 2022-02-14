using VkSchedman.Video.Enum;

namespace VkSchedman.Video.Abstractions
{
    internal interface IVideoEditor
    {
        void SetOptions(IInputOptions options);
        void SetOptions(IOutputOptions options);
        void ConvertToExtension(FileExtension extension);
        void ConcatFiles();
    }
}
