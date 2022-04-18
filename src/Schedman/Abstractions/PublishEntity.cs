using Schedman.Collections;
using System;

namespace Schedman.Abstractions
{
    public abstract class PublishEntity<TUid> : PublishEntity
    {
        public TUid Uid { get; internal set; }
    }

    public abstract class PublishEntity
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public WebSourceCollection MediaCollection { get; } = new WebSourceCollection();
    }
}
