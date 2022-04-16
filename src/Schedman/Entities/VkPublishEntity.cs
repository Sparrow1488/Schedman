using Schedman.Abstractions;
using Schedman.Collections;
using System;

namespace Schedman.Entities
{
    public class VkPublishEntity : PublishEntity<long>
    {
        private DateTime _schedule = default;
        private WebSourceCollection _webSources = new WebSourceCollection();

        public DateTime Schedule => _schedule;
        public override WebSourceCollection MediaCollection => _webSources;
    }
}
