using System.Text;
using VkSchedman.Video.Abstractions;
using VkSchedman.Video.Enum;

namespace VkSchedman.Video.Options
{
    public class OutputOptions : IOutputOptions
    {
        private StringBuilder _builder = new StringBuilder();
        private FileExtension _outputExtension = FileExtension.MP4;
        private double _fps;
        private string _outputPath = "";
        private string _outputName = "out-video";
        private VideoQuality _quality;

        public void SetFps(double fps)
        {
            if (fps < 5)
                throw new ArgumentException($"{nameof(fps)} cannot be smaller then 5!");
            _fps = fps;
        }

        public void AddCommand(string command)
        {
            throw new NotImplementedException();
        }

        public void SetOutputFileName(string name)
        {
            ThrowIfEmptyOrNull(name, nameof(name));
            _outputName = name;
        }

        public void SetOutputPath(string path)
        {
            ThrowIfEmptyOrNull(path, nameof(path));
            _outputPath = path;
        }

        public void SetOutputExtension(FileExtension extension)
        {
            ThrowIfEmptyOrNull(extension.ToString(), nameof(extension));
            _outputExtension = extension;
        }

        public string GetOutputPath() => _outputPath;
        public string GetOutputName() => _outputName;
        public FileExtension GetOutputExtension() => _outputExtension;

        public string GetResultPath() =>
            Path.Combine(_outputPath, $"{_outputName}.{_outputExtension.ToString().ToLower()}");

        public void SetVideoQuality(VideoQuality quality)
        {
            if (quality is null)
                throw new ArgumentException($"{nameof(quality)} is null");
            _quality = quality;
        }

        public string Build()
        {
            if (string.IsNullOrWhiteSpace(_outputPath))
                throw new ArgumentException($"Output path should be initialized!");

            var commands = new List<string>();
            string space = " ";
            if(_fps > 5)
                commands.Add($"-r {_fps}");
            if (_quality != null)
                commands.Add($"-s {_quality.Quality}");

            commands.ForEach(cmd => _builder.Append(cmd + space));
            var endPath = GetResultPath();
            _builder.Append($"\"{endPath}\"");
            return _builder.ToString().Trim();
        }

        private void ThrowIfEmptyOrNull(string input, string inputName, string message = "")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if (string.IsNullOrWhiteSpace(message))
                    message = $"{inputName ?? "undefined field"} cannot be null or empty!";
                throw new ArgumentException(message);
            }
        }

        
    }
}
