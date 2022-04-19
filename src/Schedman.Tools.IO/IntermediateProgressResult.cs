using System.Collections.Generic;

namespace Schedman.Tools.IO
{
    public class IntermediateProgressResult
    {
        public int TotalSize { get; set; }
        public int CurrentSize { get; set; }
        public double CurrentPercents { get; set; }
        public string CurrentPercentsStringify => CurrentPercents.ToString("0.00");
        public List<string> Messages { get; set; }
    }
}
