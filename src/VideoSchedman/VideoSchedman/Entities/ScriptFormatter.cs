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

        public ScriptFormatter CombineSources(IEnumerable<string> sources)
        {
            CombineSourcesInTxt = true;
            string filePath = $"./combined-files_{DateTime.Now.Ticks}.txt";
            using (var writer = File.CreateText(filePath))
            {
                foreach (var source in sources)
                    if(!string.IsNullOrWhiteSpace(source))
                        writer.WriteLine($"file '{source}'");
            }
            Result.CombinedSources = filePath;
            return this;
        }
    }
}
