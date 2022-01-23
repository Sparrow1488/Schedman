using Newtonsoft.Json;
using System;

namespace ScheduleVkManager.ChatBot.Entities
{
    [Serializable]
    public class BotSettings
    {
        [JsonProperty("pause")]
        public bool Pause { get; set; }
    }
}
