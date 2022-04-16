using Schedman.Abstractions;
using Schedman.Collections;

namespace Schedman.Entities
{
    public class VkPublishEntity : PublishEntity<long>
    {
        private WebSourceCollection _webSources = new WebSourceCollection();
        public override WebSourceCollection MediaCollection => _webSources;
    }
}
