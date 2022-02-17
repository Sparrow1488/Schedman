using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoSchedman.Abstractions;

namespace VideoSchedman.Entities
{
    internal class ExecutableProcess : IExecutableProcess
    {
        public Task StartAsync(IScriptBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
