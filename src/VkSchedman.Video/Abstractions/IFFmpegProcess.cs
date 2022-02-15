using System.Diagnostics;

namespace VkSchedman.Video.Abstractions
{
    public interface IFFmpegProcess
    {
        //void Start(ProcessStartInfo startInfo, string executionCommand);
        Task StartAsync(ProcessStartInfo startInfo, string executionCommand);
        ProcessStartInfo CreateDefaultStartInfo();
    }
}
