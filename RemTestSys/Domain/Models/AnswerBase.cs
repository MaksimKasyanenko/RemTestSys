using System;

namespace RemTestSys.Domain.Models
{
    public abstract class AnswerBase
    {
        public int Id { get; set; }
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
        public abstract string[] GetAddition();
        public abstract bool IsMatch(string[] data);
    }
}
