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
            if (!Directory.Exists("./files-meta"))
                Directory.CreateDirectory("./files-meta");
            string filePath = $"./files-meta/combined-files_{DateTime.Now.Ticks}.txt";
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
