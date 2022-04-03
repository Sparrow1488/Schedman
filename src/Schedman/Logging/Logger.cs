using System;

namespace VkSchedman.Logging
{
    public static class Logger
    {
        private static event Action<string> OnDebugLog;
        private static event Action<string> OnInfoLog;
        private static event Action<string> OnErrorLog;

        internal static void Debug(string message) => OnDebugLog?.Invoke(message);
        internal static void Info(string message) => OnInfoLog?.Invoke(message);
        internal static void Error(string message) => OnErrorLog?.Invoke(message);

        public static void SetDebugLogging(Action<string> action) => OnDebugLog += action;
        public static void SetInfoLogging(Action<string> action) => OnInfoLog += action;
        public static void SetErrorLogging(Action<string> action) => OnErrorLog += action;
    }
}
