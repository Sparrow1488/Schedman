using VideoSchedman.Enums;

namespace VideoSchedman.Entities
{
    public class FileMeta
    {
        public FileMeta(string rootPath)
        {
            RootPath = rootPath;
        }

        private FileMeta(string rootPath, FileType type) : this(rootPath)
        {
            Type = type;
        }

        public FileType Type { get; }
        public string RootPath { get; private set; }
        public string Name { get; private set; }
        public string Extension { get; private set; }

        public static FileMeta From(string filePath)
        {
            return new FileMeta(filePath);
        }
    }
}
