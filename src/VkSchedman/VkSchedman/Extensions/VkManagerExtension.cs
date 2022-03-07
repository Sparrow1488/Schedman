using VkNet.Exception;

namespace VkSchedman.Extensions
{
    public static class VkManagerExtension
    {
        public static void ThrowIfNotAuth(this VkManager manager)
        {
            if (!manager.IsAuthorizated)
                throw new VkAuthorizationException();
        }
    }
}
