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
    public class AccessToSessionException : Exception
    {
        public AccessToSessionException(string message) : base(message){}
    }
}