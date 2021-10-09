using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Politics
{
    public class AccountValidityPolitics
    {
        public static TimeSpan GetTerm()
        {
            return DateTime.Now.Month <= 7 ? new DateTime(DateTime.Now.Year, 8, 1) - DateTime.Now : new DateTime(DateTime.Now.Year + 1, 8, 1) - DateTime.Now;
        }
    }
}
