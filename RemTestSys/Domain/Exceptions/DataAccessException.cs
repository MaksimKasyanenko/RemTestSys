using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Exceptions
{
    public class DataAccessException:Exception
    {
        public DataAccessException(string message) : base(message) { }
    }
}
