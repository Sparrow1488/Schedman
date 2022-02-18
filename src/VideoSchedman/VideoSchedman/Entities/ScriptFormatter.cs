using VideoSchedman.Enums;

namespace VideoSchedman.Entities
{
    public class ScriptFormatter
    {
        public ScriptFormatter()
        {
            Result = new ScriptFormatterResult();
        }

        public bool CombineSourcesInTxt { get; set; }
        public ScriptFormatterResult Result { get; private set; }

        public ScriptFormatter CombineSources(IEnumerable<FileMeta> sources)
        {
            CombineSourcesInTxt = true;
            if (!Directory.Exists(Paths.Meta.Path))
                Directory.CreateDirectory(Paths.Meta.Path);
            string filePath = $"{Paths.Meta.Path}/combined-files_{DateTime.Now.Ticks}.txt";
            using (var writer = File.CreateText(filePath))
            {
                foreach (var source in sources)
                    if(!string.IsNullOrWhiteSpace(source.ToString()))
                        writer.WriteLine($"file '{source}'");
            }
            Result.CombinedSources = filePath;
            return this;
        }
    }
}
