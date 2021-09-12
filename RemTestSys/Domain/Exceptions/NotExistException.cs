using System;

namespace RemTestSys.Domain.Exceptions
{
    public class NotExistException:Exception
    {
        public NotExistException(string message) : base(message) { }
    }
}
