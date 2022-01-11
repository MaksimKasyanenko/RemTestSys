using System;

namespace RemTestSys.Domain
{
    public class AccessToTestException : Exception
    {
        public AccessToTestException(string message) : base(message){}
    }
}