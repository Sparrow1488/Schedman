using System;

namespace VkSchedman.ChatBot.Commands
{
    public class CommandResult
    {
        public CommandResult(string result = "", long dialog = 0)
        {
            Result = result;
            ToDialog = dialog;
        }
        public readonly string Result = string.Empty;
        public readonly long ToDialog;
    }
}
