using System;
using System.Collections.Generic;
using System.Text;

namespace VkSchedman.Interfaces
{
    public interface IStorableErrors
    {
        public IList<string> GetErrors();
        public void ClearErrors();
    }
}
