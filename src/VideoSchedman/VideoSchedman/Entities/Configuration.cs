using VideoSchedman.Enums;

namespace VideoSchedman.Entities
{
    public class Configuration
    {
        public Configuration()
        {
            _sources = new List<FileMeta>();
        }

        public IEnumerable<FileMeta> Sources { get => _sources; }
        public FileMeta OutputFile { get => _outputFile; }

        private List<FileMeta> _sources;
        private FileMeta _outputFile;

        public Configuration AddSrc(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File from \"{path}\" not exists");
            _sources.Add(FileMeta.From(path));
            return this;
        }

        public Configuration SaveTo(string dirPath, string name, bool createDirIfNotExists = false)
        {
            if (string.IsNullOrWhiteSpace(dirPath) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(dirPath)} and {nameof(name)} cannot be empty or null!");
            if (createDirIfNotExists)
                Directory.CreateDirectory(dirPath);
            if (!Directory.Exists(dirPath))
                throw new DirectoryNotFoundException($"\"{dirPath}\" not found!");

            _outputFile = new FileMeta(dirPath, FileType.Video)
            {
                Name = name,
                Extension = "mp4"
            };
            return this;
        }

        public Configuration SaveAs(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException($"{nameof(extension)} cannot be null!");
            _outputFile.Extension = extension;
            return this;
        }

        public Configuration Quality(VideoQuality quality)
        {
            if (quality is null)
                throw new ArgumentException($"{nameof(quality)} cannot be null!");
            _outputFile.VideoQuality = quality;
            return this;
        }
    }
}
