using VkNet.Exception;

namespace Schedman.Extensions
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
