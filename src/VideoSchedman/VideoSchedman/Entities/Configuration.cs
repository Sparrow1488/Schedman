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
            _sources.Add(FileMeta.From(path));
            return this;
        }

        public Configuration SaveTo(string dirPath, string name)
        {
            _outputFile = new FileMeta(dirPath, FileType.Video)
            {
                Name = name,
                Extension = "mp4"
            };
            return this;
        }
    }
}
