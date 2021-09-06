using System;

namespace RemTestSys.Domain.Exceptions
{
    public class NonExsistException:Exception
    {
        public NonExsistException(string message) : base(message) { }
    }
}
