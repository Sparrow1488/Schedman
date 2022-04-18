using Schedman.Abstractions;
using Schedman.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Schedman.Entities
{
    public class PublishResult
    {
        public PublishResult(IList<SchedmanException> exceptions, IEnumerable<PublishEntity> entities)
        {
            _exceptions = exceptions;
            FailedToUpload = entities;
        }

        private readonly IList<SchedmanException> _exceptions;

        public bool AllSuccess => !_exceptions.Any();
        public IEnumerable<PublishEntity> FailedToUpload { get; }

        public PublishResult ThrowIfHasFails()
        {
            if(_exceptions.Count > 0)
            {
                throw _exceptions.First();
            }
            return this;
        }
    }
}
