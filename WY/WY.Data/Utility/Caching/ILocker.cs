using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WY.Data.Utility.Caching
{
    public interface ILocker
    {
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
