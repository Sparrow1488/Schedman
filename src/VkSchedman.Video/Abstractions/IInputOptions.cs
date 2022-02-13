namespace VkSchedman.Video.Abstractions
{
    public interface IInputOptions : IOptions
    {
        void AddSource(string sourcePath);
        void SetStartPosition(TimeSpan start);
        void SetEndPosition(TimeSpan end);
    }
}
