using System;

namespace RemTestSys.Domain.Exceptions
{
    public class NonExistException:Exception
    {
        public NonExistException(string message) : base(message) { }
    }
}
