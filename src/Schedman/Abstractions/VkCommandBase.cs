using VkNet.Abstractions;

namespace Schedman.Abstractions
{
    internal abstract class VkCommandBase : CommandBase
    {
        public VkCommandBase(IVkApi api)
        {
            Api = api;
        }

        protected IVkApi Api { get; }
    }
}
