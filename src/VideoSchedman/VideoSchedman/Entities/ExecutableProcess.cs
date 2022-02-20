using System.Diagnostics;
using VideoSchedman.Abstractions;

namespace VideoSchedman.Entities
{
    internal class ExecutableProcess : IExecutableProcess
    {
        public ExecutableProcess(string ffmpegPath)
        {
            _ffmpegPath = ffmpegPath;
        }

        private string _ffmpegPath = string.Empty;

        public async Task StartAsync(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException(nameof(command));

            var startInfo = CreateStartInfoDefault();

            startInfo.Arguments = command;
            using (var process = new Process() { StartInfo = startInfo })
            {
                if (process is null)
                    throw new Exception("Process not started");
                process.Start();
                await process.WaitForExitAsync();
            }
        }

        public async Task StartDebugAsync(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException(nameof(command));

            var startInfo = CreateStartInfoDebug();

            startInfo.Arguments = command;
            using (var process = new Process() { StartInfo = startInfo })
            {
                if (process is null)
                    throw new Exception("Process not started");
                process.EnableRaisingEvents = true;
                process.Start();
                await process.WaitForExitAsync();
                //var outString = process.StandardError.ReadToEnd();
                //if(!string.IsNullOrWhiteSpace(outString))
                //    Console.WriteLine(outString);
                process.StandardError.Dispose();
            }
        }

        private ProcessStartInfo CreateStartInfoDefault()
        {
            var ffmpegStartInfo = new ProcessStartInfo()
            {
                FileName = _ffmpegPath,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                CreateNoWindow = true,
                UseShellExecute = true
            };
            return ffmpegStartInfo;
        }

        private ProcessStartInfo CreateStartInfoDebug()
        {
            var ffmpegStartInfo = CreateStartInfoDefault();
            //ffmpegStartInfo.RedirectStandardError = true;
            //ffmpegStartInfo.RedirectStandardOutput = true;
            return ffmpegStartInfo;
        }
        
    }
}
