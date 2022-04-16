using Schedman.Abstractions;
using Schedman.Collections;
using System;

namespace Schedman.Entities
{
    public class VkPublishEntity : PublishEntity<long>
    {
        private long _uid = -1;
        private string _message = string.Empty;
        private DateTime _schedule = default;
        private WebSourceCollection _webSources = new WebSourceCollection();

        public override long Uid => _uid;
        public override string Message => _message;
        public DateTime Schedule => _schedule;
        public override WebSourceCollection MediaCollection => _webSources;
    }
}
