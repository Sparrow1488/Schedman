using VkSchedman.Video.Options;

namespace VkSchedman.Video.Abstractions
{
    public interface IInputOptions : IOptions
    {
        void AddSource(string sourcePath);
        IEnumerable<string> GetSources();
        void SetStartPosition(TimeSpan start);
        void SetEndPosition(TimeSpan end);
        InputOptionsSettings GetOptionsSettings();
    }
}
