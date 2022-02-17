using VideoSchedman.Entities;

namespace VideoSchedman.Abstractions
{
    public interface IScriptBuilder
    {
        string Build(Configuration config, Action<ScriptFormat> format);
        string Build(Configuration config);
        IScriptBuilder ChangeFormat(Action<ScriptFormat> format);
    }
    
}
