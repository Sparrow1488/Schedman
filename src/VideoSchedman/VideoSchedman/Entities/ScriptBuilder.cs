using VideoSchedman.Abstractions;

namespace VideoSchedman.Entities
{
    public class ScriptBuilder : IScriptBuilder
    {
        public string Build(Configuration config, Action<ScriptFormat> format)
        {
            throw new NotImplementedException();
        }

        public string Build(Configuration config)
        {
            throw new NotImplementedException();
        }

        public IScriptBuilder ChangeFormat(Action<ScriptFormat> format)
        {
            throw new NotImplementedException();
        }
    }
}
