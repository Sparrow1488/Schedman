namespace VkSchedman.Video.Abstractions
{
    public interface IOutputOptions : IOptions
    {
        void SetOutputPath(string path);
        string GetOutputPath();
        void SetFps(double fps);
        void SetVideoSize(int width, int height);
    }
}
