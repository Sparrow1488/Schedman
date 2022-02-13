using System.Diagnostics;
using System.Text;
using VkSchedman.Video.Abstractions;

namespace VkSchedman.Video
{
    public class VideoEditor
    {
        public VideoEditor(string ffmpegPath, string outputFilePath = "")
        {
            if (!File.Exists(ffmpegPath))
                throw new ArgumentException("File not exists!");
            FFmpegPath = ffmpegPath;
            OutputFilePath = outputFilePath;
        }

        public readonly string FFmpegPath;
        public string OutputFilePath { get; set; }
        private List<string> _outputOptions = new List<string>();
        private IInputOptions _inputOptions;

        public void SetInputOptions(IInputOptions options)
        {
            if(options is null)
                throw new ArgumentNullException($"nameof(options) cannot be null!");
            _inputOptions = options;
        }

        public void ConvertToExtension()
        {
            var command = BuildOutputCommand();
            var ffmpegStartInfo = new ProcessStartInfo()
            {
                FileName = FFmpegPath,
                Arguments = command,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                CreateNoWindow = true,
                UseShellExecute = false
            };

            StartFFmpegProcess(ffmpegStartInfo);
        }

        private void StartFFmpegProcess(ProcessStartInfo startInfo)
        {
            using (var process = new Process() { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        private string BuildOutputCommand()
        {
            if (string.IsNullOrWhiteSpace(OutputFilePath))
                throw new ArgumentException($"Not set {nameof(OutputFilePath)}");
            if (_inputOptions is null)
                throw new ArgumentException($"{nameof(_inputOptions)} cannot be null!");

            var builder = new StringBuilder();
            string space = " ";
            builder.Append(_inputOptions.Build() + space);
            _outputOptions.Add(OutputFilePath);
            _outputOptions.ForEach(opt => builder.Append(opt + space));
            return builder.ToString().Trim();
        }
    }
}