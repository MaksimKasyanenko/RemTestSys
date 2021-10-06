using System;

namespace RemTestSys.Domain.Models
{
    public abstract class AnswerBase
    {
        public int Id { get; set; }
        public virtual string RightText { get; set; }
        public abstract string[] GetAdditiveData();
        public abstract bool IsMatch(string[] data);
    }
}
