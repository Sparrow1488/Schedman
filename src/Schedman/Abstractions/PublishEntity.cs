using Schedman.Collections;

namespace Schedman.Abstractions
{
    public abstract class PublishEntity<TUid> : PublishEntity
    {
        private TUid _uid = default;
        public TUid Uid => _uid;

        internal void SetUid(TUid uid) =>
            _uid = uid;
    }

    public abstract class PublishEntity
    {
        private string _message = string.Empty;
        public string Message => _message;
        public abstract WebSourceCollection MediaCollection { get; }

        public void SetMessage(string message) =>
            _message = message;
    }
}
