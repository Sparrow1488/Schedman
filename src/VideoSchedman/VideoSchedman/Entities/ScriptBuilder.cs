using System.Text;
using VideoSchedman.Abstractions;

namespace VideoSchedman.Entities
{
    public class ScriptBuilder : IScriptBuilder
    {
        private List<string> _input = new List<string>();
        private List<string> _middle = new List<string>();
        private List<string> _output = new List<string>();

        public string Build(Configuration config)
        {
            foreach (var source in config.Sources)
                _input.Add($"-i \"{source}\"");
            _output.Add($"\"{config.OutputFile}\"");

            var builder = new StringBuilder();
            builder.AppendJoin(" ", _input);
            builder.AppendJoin(" ", _middle);
            builder.AppendJoin(" ", _output);
            return builder.ToString();
        }

        public string Build(Configuration config, Action<ScriptFormat> format)
        {
            throw new NotImplementedException();
        }

        public IScriptBuilder ChangeFormat(Action<ScriptFormat> format)
        {
            throw new NotImplementedException();
        }
    }
}
