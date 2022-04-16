using Schedman.Collections;
using System;

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
        private DateTime _createdAt = default;

        public string Message => _message;
        public DateTime CreatedAt => _createdAt;
        public abstract WebSourceCollection MediaCollection { get; }

        public void SetMessage(string message) =>
            _message = message;
        public void SetCreatedDate(DateTime date) =>
            _createdAt = date;
    }
}
