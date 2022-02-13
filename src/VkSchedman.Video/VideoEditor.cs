using System.Diagnostics;
using System.Text;

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
        private List<string> _inputOptions = new List<string>();
        private List<string> _outputOptions = new List<string>();

        public void AddVideo(string videoPath)
        {
            if (string.IsNullOrWhiteSpace(videoPath))
                throw new ArgumentException($"{nameof(videoPath)} cannot be empty or null!");
            if (!File.Exists(videoPath))
                throw new FileLoadException($"File ('{videoPath}') not exists");
            if (!_inputOptions.Contains("-y"))
                _inputOptions.Add("-y");

            _inputOptions.Add($"-i {videoPath}");
        }

        public void ConvertToAvi()
        {
            var command = BuildOutputCommand();
            var ffmpegStartInfo = new ProcessStartInfo()
            {
                FileName = FFmpegPath,
                Arguments = command,
                WorkingDirectory = @"D:\games\ffmpeg\",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var process = new Process() { StartInfo = ffmpegStartInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        private string BuildOutputCommand()
        {
            if (string.IsNullOrWhiteSpace(OutputFilePath))
                throw new ArgumentException($"Not set {nameof(OutputFilePath)}");

            var builder = new StringBuilder();
            string betweenTemplate = " ";
            _inputOptions.ForEach(opt => builder.Append(opt + betweenTemplate));
            _outputOptions.Add(OutputFilePath);
            _outputOptions.ForEach(opt => builder.Append(opt + betweenTemplate));
            return builder.ToString().Trim();
        }
    }
}