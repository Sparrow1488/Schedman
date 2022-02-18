using System.Text;
using VideoSchedman.Abstractions;

namespace VideoSchedman.Entities
{
    public class ScriptBuilder : IScriptBuilder
    {
        private List<string> _input = new List<string>();
        private List<string> _middle = new List<string>();
        private List<string> _output = new List<string>();
        private ScriptFormatter _formatter = new ScriptFormatter();

        public string Build(Configuration config)
        {
            return Build(config, formatter => new ScriptFormatter());
        }

        public string Build(Configuration config, Action<ScriptFormatter> format)
        {
            format(_formatter);
            var builder = new StringBuilder();

            SetValuesFromConfigUsingFormatter(config);
            builder.AppendJoin(" ", _input);
            builder.AppendJoin(" ", _middle);
            if (_middle.Count < 1)
                builder.Append(" ");
            builder.AppendJoin(" ", _output);
            return builder.ToString();
        }

        public IScriptBuilder ChangeFormat(Action<ScriptFormatter> format)
        {
            format(_formatter);
            return this;
        }

        public IScriptBuilder ConfigureInputs(Action<IList<string>> commands)
        {
            commands(_input);
            return this;
        }

        public IScriptBuilder ConfigureOutputs(Action<IList<string>> commands)
        {
            commands(_output);
            return this;
        }

        public IScriptBuilder Clean()
        {
            _input = new List<string>();
            _output = new List<string>();
            _middle = new List<string>();
            _formatter = new ScriptFormatter();
            return this;
        }

        private void SetValuesFromConfigUsingFormatter(Configuration config)
        {
            if (_formatter.CombineSourcesInTxt)
                _input.Add($"-i \"{_formatter.Result.CombinedSources}\"");
            else SetInputScriptParams(config);
            SetOutputScriptParams(config);
        }

        private void SetInputScriptParams(Configuration config)
        {
            foreach (var source in config.Sources)
                _input.Add($"-i \"{source}\"");
        }

        private void SetOutputScriptParams(Configuration config)
        {
            _output.Add($"\"{config.OutputFile}\"");
        }
        
    }
}
