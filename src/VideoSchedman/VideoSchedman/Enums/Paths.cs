namespace VideoSchedman.Enums
{
    public class Paths
    {
        private Paths(string path)
        {
            Directory.CreateDirectory(path);
            Path = path;
        }

        //public static void SetProject(string projectName)
        //{
        //    ProjectPath.Path = string.Format(ProjectPath.Path, projectName);
        //    FilesCache.Path = string.Format(FilesCache.Path, ProjectPath.Path);
        //    ConvertedFiles.Path = string.Format(ConvertedFiles.Path, ProjectPath.Path);
        //    TsFiles.Path = string.Format(TsFiles.Path, ProjectPath.Path);
        //}

        public string Path { get; private set; } // относительно запускаемого проекта

        public static readonly Paths Meta = new Paths("./meta-files");
        public static readonly Paths Resources = new Paths("./Resources");
        public static readonly Paths FilesCache = new Paths("./cached-files");
        public static readonly Paths ConvertedFiles = new Paths($"{FilesCache}/converted");
        public static readonly Paths TsFiles = new Paths($"{FilesCache}/ts");
        public static readonly Paths OutputFiles = new Paths("./output-files");

        public override string ToString() => Path;
    }
}
