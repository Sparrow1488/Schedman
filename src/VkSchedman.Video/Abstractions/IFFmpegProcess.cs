using System.Diagnostics;

namespace VkSchedman.Video.Abstractions
{
    public interface IFFmpegProcess
    {
        Task StartAsync(ProcessStartInfo startInfo, string executionCommand);
        ProcessStartInfo CreateDefaultStartInfo();
    }
}
