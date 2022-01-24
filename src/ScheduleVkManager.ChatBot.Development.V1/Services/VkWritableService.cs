using ScheduleVkManager.ChatBot.Services.Interfaces;
using VkNet.Abstractions;

namespace ScheduleVkManager.ChatBot.Services
{
    public class VkWritableService : IWritableService
    {
        public VkWritableService(IVkApi api)
        {
            _api = api;
        }

        private readonly IVkApi _api;

        public void Write(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
