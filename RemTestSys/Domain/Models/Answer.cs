using System;

namespace RemTestSys.Domain.Models
{
    public abstract class Answer
    {
        public int Id { get; set; }
        public virtual string RightText { get; set; }
        public abstract string[] GetAdditiveData();
        public abstract bool IsMatch(string[] data);
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public static Answer Create(string text, bool caseMatters)
        {
            return new TextAnswer {
                RightText=text,
                CaseMatters = caseMatters
            };
        }
    }
}
