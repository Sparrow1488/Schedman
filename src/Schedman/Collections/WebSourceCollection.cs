using Schedman.Abstractions;
using Schedman.Entities;
using Schedman.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Schedman.Collections
{
    public class WebSourceCollection : IEnumerable<WebSource>
    {
        private readonly IList<WebSource> _sourcesList = new List<WebSource>();

        public IEnumerable<WebImage> Images =>
            (IEnumerable<WebImage>)_sourcesList.Where(m => m.Type.Equals(WebSourceType.Image));

        public IEnumerator<WebSource> GetEnumerator() => _sourcesList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _sourcesList.GetEnumerator();
    }
}
