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

        public FileType Type { get; } = FileType.Undefined;
        public string RootPath { get; internal set; }
        public string Name { get; internal set; }
        public string Extension { get; internal set; }
        public VideoQuality VideoQuality { get; internal set; } = VideoQuality.Undefined;
        public static readonly FileMeta Empty = new FileMeta("./", FileType.Undefined);

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static FileMeta From(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"{nameof(filePath)} cannot be empty or null!");
            if (!IsFilePathCharsValid(filePath))
                throw new InvalidOperationException($"{filePath} contains invalid chars unusable in ffmpeg (' ' ', '$') or file path too long");
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

        /// <exception cref="ArgumentException"></exception>
        public static bool IsFilePathCharsValid(string filePath)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"{nameof(filePath)} cannot be empty or null!");
            if (filePath.Contains("$") || filePath.Contains("'"))
                isValid = false;
            return isValid;
        }

        public override string ToString()
        {
            return $"{Path.Combine(RootPath, Name)}.{Extension}".Replace('/', '\\');
        }

        // TODO: переписать
        private static FileType SelectType(string extension)
        {
            var result = FileType.Undefined;

            if (new string[] { "mp4", "avi", "ts" }.Contains(extension))
                result = FileType.Video;
            if (new string[] { "mp3", "flac" }.Contains(extension))
                result = FileType.Audio;
            return result;
        }
    }
}
