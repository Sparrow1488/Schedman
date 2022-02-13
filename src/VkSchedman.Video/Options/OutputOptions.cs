using System.Text;
using VkSchedman.Video.Abstractions;

namespace VkSchedman.Video.Options
{
    public class OutputOptions : IOutputOptions
    {
        private StringBuilder _builder = new StringBuilder();
        private double _fps;
        private string _outputPath = "";
        private int _height;
        private int _width;

        public void SetFps(double fps)
        {
            if (fps < 5)
                throw new ArgumentException($"{nameof(fps)} cannot be smaller then 5!");
            _fps = fps;
        }

        public void SetOutputPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(path)} cannot be null or empty!");
            _outputPath = path;
        }

        public string GetOutputPath() => _outputPath;

        public void SetVideoSize(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException($"{nameof(width)} or {nameof(height)} should be possitive!");
            _height = height;
            _width = width;
        }

        public string Build()
        {
            if (string.IsNullOrWhiteSpace(_outputPath))
                throw new ArgumentException($"Output path should be initialized!");

            var commands = new List<string>();
            string space = " ";
            if(_fps > 5)
                commands.Add($"-f {_fps}");
            if (_height > 0 && _width > 0)
                commands.Add($"-s {_width}x{_height}");
            commands.ForEach(cmd => _builder.Append(cmd + space));
            _builder.Append($"\"{_outputPath}\"");
            return _builder.ToString().Trim();
        }

    }
}
