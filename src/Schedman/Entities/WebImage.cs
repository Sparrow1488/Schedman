using Schedman.Abstractions;
using Schedman.Enums;
using System;

namespace Schedman.Entities
{
    public class WebImage : WebSource
    {
        public WebImage(string url) : base(url) { }
        public override WebSourceType Type => WebSourceType.Image;
    }
}
