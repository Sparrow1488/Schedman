using VkSchedman.ChatBot.Commands;
using VkSchedman.ChatBot.Services.Interfaces;
using System;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VkSchedman.ChatBot.Services
{
    public class VkWritableService : IWritableService
    {
        public VkWritableService(IVkApi vkApi)
        {
            _vkApi = vkApi;
        }

        private readonly IVkApi _vkApi;

        public void Write(CommandResult input)
        {
            if (IsValid(input)) {
                _vkApi.Messages.Send(new MessagesSendParams() {
                    RandomId = DateTime.Now.Millisecond,
                    PeerId = input.ToDialog,
                    Message = input.Result
                });
            }
        }

        private bool IsValid(CommandResult input)
        {
            bool result = true;
            if(input.ToDialog == 0) {
                throw new ArgumentException("Не присвоен идентификатор диалога");
            }
            return result;
        }
    }
}
