using System;

namespace RemTestSys.Domain
{
    public class AccessToExamException : Exception
    {
        public AccessToExamException(string message) : base(message){}
    }
    public class AccessToResultException : Exception
    {
        public AccessToResultException(string message) : base(message){}
    }
}