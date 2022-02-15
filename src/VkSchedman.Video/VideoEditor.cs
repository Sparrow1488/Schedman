using System.Diagnostics;
using System.Text;
using VkSchedman.Video.Abstractions;
using VkSchedman.Video.Enum;

namespace VkSchedman.Video
{
    public class VideoEditor : IVideoEditor
    {
        public VideoEditor(string ffmpegPath)
        {
            if (!File.Exists(ffmpegPath))
                throw new ArgumentException("File not exists!");
            FFmpegPath = ffmpegPath;
            _process = new FFmpegProcess(FFmpegPath);
        }

        public readonly string FFmpegPath;
        public bool IsDebugEnable = false;
        private IOutputOptions _outputOptions;
        private IInputOptions _inputOptions;
        private IFFmpegProcess _process;

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

        public async Task ConvertToExtensionAsync(FileExtension extension)
        {
            _outputOptions.SetOutputExtension(extension);
            var command = BuildOutputCommand();

            await StartProcessAsync(command);
        }

        public async Task ConcatFilesAsync()
        {
            var settings = _inputOptions.GetOptionsSettings();
            settings.IsCombineSourcesInTxt = true;
            _inputOptions.AddCommand("-safe 0");
            _inputOptions.AddCommand("-f concat");
            var command = BuildOutputCommand();
            await StartProcessAsync(command);
        }

        private async Task StartProcessAsync(string command)
        {
            var startInfo = _process.CreateDefaultStartInfo();
            if (IsDebugEnable)
                startInfo.UseShellExecute = true;
            await _process.StartAsync(startInfo, command);
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