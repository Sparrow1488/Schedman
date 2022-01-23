using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ScheduleVkManager.ChatBot.Entities
{
    [Serializable]
    public class VkCallback
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("object")]
        public JObject Object { get; set; }
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

    }
}
