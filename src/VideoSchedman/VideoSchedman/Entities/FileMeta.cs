using VideoSchedman.Enums;

namespace VideoSchedman.Entities
{
    public class FileMeta
    {
        internal FileMeta(string rootPath, FileType type)
        {
            RootPath = rootPath;
            Type = type;
        }

        public FileType Type { get; }
        public string RootPath { get; internal set; }
        public string Name { get; internal set; }
        public string Extension { get; internal set; }
        public static readonly FileMeta Empty = new FileMeta("./", FileType.Undefined);

        public static FileMeta From(string filePath)
        {
            var info = new FileInfo(filePath);
            string correctExtension = info.Extension.Remove(0, 1).ToLower();
            string correctName = info.Name.Remove(info.Name.Length - info.Extension.Length, info.Extension.Length);
            var metaType = SelectType(correctExtension);
            var meta = new FileMeta(filePath, metaType)
            {
                Extension = correctExtension,
                Name = correctName,
                RootPath = info.DirectoryName ?? string.Empty,
            };
            return meta;
        }

        // TODO: переписать
        private static FileType SelectType(string extension)
        {
            var result = FileType.Undefined;

            if (new string[] { "mp4", "avi" }.Contains(extension))
                result = FileType.Video;
            if (new string[] { "mp3", "flac" }.Contains(extension))
                result = FileType.Audio;
            return result;
        }
    }
}
