using System.Text;
using VideoSchedman.Enums;

namespace VideoSchedman.Entities
{
    public class ScriptFormatter
    {
        public ScriptFormatter()
        {
            Result = new ScriptFormatterResult();
        }

        public bool IsCombinedSourcesInTxt { get; private set; }
        public bool IsCombinedSources { get; private set; }
        public ScriptFormatterResult Result { get; private set; }

        public ScriptFormatter CombineSourcesInTxt(IEnumerable<FileMeta> sources)
        {
            IsCombinedSourcesInTxt = true;
            if (!Directory.Exists(Paths.Meta.Path))
                Directory.CreateDirectory(Paths.Meta.Path);
            string filePath = $"{Paths.Meta.Path}/combined-files_{DateTime.Now.Ticks}.txt";
            using (var writer = File.CreateText(filePath))
            {
                foreach (var source in sources)
                    if(!string.IsNullOrWhiteSpace(source.ToString()))
                        writer.WriteLine($"file '{source}'");
            }
            Result.CombinedSourcesInTxt = filePath;
            return this;
        }

        public ScriptFormatter CombineSources(IEnumerable<FileMeta> sources)
        {
            IsCombinedSources = true;
            var builder = new StringBuilder();
            sources.ToList().ForEach(source => builder.Append($"-i \"{source}\""));
            Result.CombinedSources = builder.ToString();
            return this;
        }
    }
}
