using VkSchedman.Video.Enum;

namespace VkSchedman.Video.Abstractions
{
    public interface IOutputOptions : IOptions
    {
        void SetOutputPath(string path);
        void SetOutputFileName(string name);
        void SetOutputExtension(FileExtension extension);
        string GetOutputPath();
        string GetOutputName();
        FileExtension GetOutputExtension();
        string GetResultPath();
        void SetFps(double fps);
        void SetVideoSize(int width, int height);
    }
}
