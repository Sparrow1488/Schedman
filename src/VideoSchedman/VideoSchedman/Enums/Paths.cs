namespace VideoSchedman.Enums
{
    public class Paths
    {
        private Paths(string path)
        {
            Directory.CreateDirectory(path);
            Path = path;
        }

        public string Path { get; private set; } // относительно запускаемого проекта

        public static readonly Paths Meta = new Paths("./meta-files");
        public static readonly Paths FilesCache = new Paths("./cached-files");
    }
}
