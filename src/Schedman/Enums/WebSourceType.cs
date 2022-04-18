using System;
using System.Collections.Generic;
using System.Text;

namespace Schedman.Enums
{
    public class WebSourceType
    {
        private WebSourceType(string typeName) =>
            TypeName = typeName;

        public string TypeName { get; }

        public static readonly WebSourceType Image = new WebSourceType(nameof(Image));
        public static readonly WebSourceType Video = new WebSourceType(nameof(Video));

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is WebSourceType media)
                equals = TypeName == media.TypeName;
            return equals;
        }
    }
}
