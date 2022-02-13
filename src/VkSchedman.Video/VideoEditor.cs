using System.Diagnostics;
using System.Text;
using VkSchedman.Video.Abstractions;
using VkSchedman.Video.Enum;

namespace VkSchedman.Video
{
    public class VideoEditor
    {
        public VideoEditor(string ffmpegPath, string outputFilePath = "")
        {
            if (!File.Exists(ffmpegPath))
                throw new ArgumentException("File not exists!");
            FFmpegPath = ffmpegPath;
        }

        public readonly string FFmpegPath;
        private IOutputOptions _outputOptions;
        private IInputOptions _inputOptions;

        public void SetOptions(IInputOptions options)
        {
            if(options is null)
                throw new ArgumentNullException($"{nameof(options)} cannot be null!");
            _inputOptions = options;
        }

        public void SetOptions(IOutputOptions options)
        {
            if (options is null)
                throw new ArgumentNullException($"{nameof(options)} cannot be null!");
            _outputOptions = options;
        }

        public void ConvertToExtension(FileExtension extension)
        {
            var outPath = _outputOptions.GetOutputPath();
            var extensionName = new FileInfo(outPath).Extension;
            var outFileWithoutExtension = outPath.Remove(outPath.Length - extensionName.Length);
            outFileWithoutExtension += $".{extension.ToString().ToLower()}";
            _outputOptions.SetOutputPath(outFileWithoutExtension);

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
            if (_outputOptions is null)
                throw new ArgumentException($"{nameof(_outputOptions)} cannot be null!");
            if (string.IsNullOrWhiteSpace(_outputOptions.GetOutputPath()))
                throw new ArgumentException($"OutputFilePath should be initialized!");
            if (_inputOptions is null)
                throw new ArgumentException($"{nameof(_inputOptions)} cannot be null!");

            var builder = new StringBuilder();
            string space = " ";
            builder.Append(_inputOptions.Build() + space);
            builder.Append(_outputOptions.Build() + space);
            return builder.ToString().Trim();
        }
    }
}