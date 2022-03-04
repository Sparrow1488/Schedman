using Newtonsoft.Json;

namespace VideoSchedman.Enums
{
    public class Paths
    {
        [JsonConstructor]
        private Paths(string path)
        {
            Directory.CreateDirectory(path);
            Path = path;
        }

        public string Path { get; private set; } // относительно запускаемого проекта

        public static readonly Paths Meta = new Paths("./meta-files");
        public static readonly Paths Resources = new Paths("./Resources");
        public static readonly Paths FilesCache = new Paths("./cached-files");
        public static readonly Paths Projects = new Paths("./projects");
        public static Paths ConvertedFiles;
        public static Paths CurrentProject;
        public static Paths TsFiles;
        public static readonly Paths OutputFiles = new Paths("./output-files");

        public override string ToString() => Path;
        public static void CreateProject(string projectName)
        {
            CurrentProject = new Paths(System.IO.Path.Combine(Projects.Path, projectName));
            ConvertedFiles = new Paths(System.IO.Path.Combine(CurrentProject.Path, "converted"));
            TsFiles = new Paths(System.IO.Path.Combine(CurrentProject.Path, "ts"));
        }
    }
}
