using System.Diagnostics;
using VkSchedman.Video.Abstractions;

namespace VkSchedman.Video
{
    internal class FFmpegProcess : IFFmpegProcess
    {
        public FFmpegProcess(string ffmpegPath)
        {
            if (string.IsNullOrWhiteSpace(ffmpegPath))
                throw new ArgumentException($"{nameof(ffmpegPath)} cannot be empty or null!");
            _ffmpegPath = ffmpegPath;
        }

        private string _ffmpegPath = "";

        public ProcessStartInfo CreateDefaultStartInfo()
        {
            var ffmpegStartInfo = new ProcessStartInfo()
            {
                FileName = _ffmpegPath,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                CreateNoWindow = true,
                UseShellExecute = false
            };
            return ffmpegStartInfo;
        }

        public async Task StartAsync(ProcessStartInfo startInfo, string executionCommand)
        {
            startInfo.Arguments = executionCommand;
            
            using (var process = new Process() { StartInfo = startInfo })
            {
                if (process is null)
                    throw new Exception("Process not started");
                process.EnableRaisingEvents = true;
                process.Start();
                await process.WaitForExitAsync();
            }
        }
    }
}
