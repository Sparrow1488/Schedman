using Newtonsoft.Json;
using System;

namespace VkSchedman.ChatBot.Entities
{
    [Serializable]
    public class BotSettings
    {
        [JsonProperty("pause")]
        public bool Pause { get; set; }
    }
}
