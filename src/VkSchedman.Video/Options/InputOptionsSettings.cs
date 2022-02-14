using System.Diagnostics.CodeAnalysis;

namespace VkSchedman.Video.Options
{
    public class InputOptionsSettings
    {
        public bool IsCombineSourcesInTxt { get; set; }

        /// <returns>Path to txt</returns>
        public string CombineSourcesInTxt(IEnumerable<string> sources, [AllowNull] string root = null)
        {
            var path = Path.Combine(root ?? Directory.GetCurrentDirectory(), "files.txt");
            using (var sw = File.CreateText(path))
                foreach (var src in sources)
                    sw.WriteLine($"file '{src}'");
            return path;
        }
    }
}
