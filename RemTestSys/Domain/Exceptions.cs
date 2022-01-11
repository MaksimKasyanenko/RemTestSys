using System;

namespace RemTestSys.Domain
{
    public class AccessToExamException : Exception
    {
        public AccessToExamException(string message) : base(message){}
    }
}