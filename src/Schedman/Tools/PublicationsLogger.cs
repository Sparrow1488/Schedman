using Newtonsoft.Json;
using VkSchedman.Entities;
using System;
using System.IO;

namespace VkSchedman.Tools
{
    public class PublicationsLogger
    {
        public void LogNotPublicated(CreatePost post)
        {
            if (post == null)
                throw new ArgumentNullException("Post was null!");

            var currentPath = Directory.GetCurrentDirectory();
            var logDirPath = Path.Combine(currentPath, "post-log");
            if(!Directory.Exists(logDirPath))
                Directory.CreateDirectory(logDirPath);

            var nowTicks = DateTime.Now.Ticks;
            var logName = Path.Combine(logDirPath, $"not_publicated_{nowTicks}.txt");
            using (var writer = File.CreateText(logName))
            {
                var json = JsonConvert.SerializeObject(post, Formatting.Indented);
                writer.Write(json);
            }
        }

        private void LogPost()
        {
            throw new NotImplementedException();

            StreamWriter writer;
            var currentPath = Directory.GetCurrentDirectory();
            var logDirPath = Path.Combine(currentPath, "post-log");
            var logFilePath = Path.Combine(logDirPath, "logging.txt");
            if (!File.Exists(logFilePath))
                writer = File.CreateText(logFilePath);
            else writer = new StreamWriter(File.OpenWrite(logFilePath));
        }
    }
}
