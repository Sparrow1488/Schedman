using Newtonsoft.Json;

namespace ScheduleVkManager.ChatBot.Entities
{
    public class BotSettings
    {
        [JsonProperty("pause")]
        public bool Pause { get; set; }
    }
}
