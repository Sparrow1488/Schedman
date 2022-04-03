using System;

namespace VkSchedman.Examples.Entities
{
    public static class Logger
    {
        private static event Action<string> OnDebugLog;
        private static event Action<string> OnInfoLog;
        private static event Action<string> OnErrorLog;
        private static event Action<Exception> OnExceptionLog;

        public static void Debug(string message) => OnDebugLog?.Invoke(message);
        public static void Info(string message) => OnInfoLog?.Invoke(message);
        public static void Error(string message) => OnErrorLog?.Invoke(message);
        public static void Exception(Exception ex) => OnExceptionLog?.Invoke(ex);

        public static void SetDebugLogging(Action<string> action) => OnDebugLog += action;
        public static void SetInfoLogging(Action<string> action) => OnInfoLog += action;
        public static void SetErrorLogging(Action<string> action) => OnErrorLog += action;
        public static void SetExceptionLogging(Action<Exception> action) => OnExceptionLog += action;
    }
}
