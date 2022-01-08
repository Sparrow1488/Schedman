using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleVkManager.Interfaces
{
    public interface IStorableErrors
    {
        public IList<string> GetErrors();
        public void ClearErrors();
    }
}
