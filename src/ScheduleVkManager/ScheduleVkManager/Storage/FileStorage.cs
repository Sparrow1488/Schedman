using Newtonsoft.Json;
using ScheduleVkManager.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScheduleVkManager.Storage
{
    internal class FileStorage
    {
        private JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        public string SaveAlbum(Album album)
        {
            var dirInfo = Directory.CreateDirectory("./Storage");
            string filePath = Path.Combine(dirInfo.FullName, $"album_{DateTime.Now.Ticks}.txt");
            var serialize = string.Empty;
            using (File.CreateText(filePath)) {
                serialize = JsonConvert.SerializeObject(album, _settings);
            }
            
            File.WriteAllText(filePath, serialize);

            return filePath;
        }

        public Album GetAlbum(string key)
        {
            var albumJson = File.ReadAllText(key);
            return JsonConvert.DeserializeObject<Album>(albumJson, _settings);
        }
    }
}
