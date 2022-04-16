using Schedman.Collections;

namespace Schedman.Abstractions
{
    public abstract class PublishEntity<TUid> : PublishEntity
    {
        public abstract TUid Uid { get; }
    }

    public abstract class PublishEntity
    {
        public abstract string Message { get; }
        public abstract WebSourceCollection MediaCollection { get; }
    }
}
