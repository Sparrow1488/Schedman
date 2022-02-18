using VideoSchedman.Entities;

namespace VideoSchedman.Abstractions
{
    public interface IScriptBuilder
    {
        string Build(Configuration config, Action<ScriptFormatter> format);
        string Build(Configuration config);
        IScriptBuilder ChangeFormat(Action<ScriptFormatter> format);
    }
    
}
