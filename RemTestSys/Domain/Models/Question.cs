using System;

namespace RemTestSys.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public Answer Answer { get; set; }
        public double Cast{get;set;}

        public static Question Create(string text, string subText, int testId)
        {
            if (text == null || text == "") throw new InvalidOperationException();
            return new Question {
                Text = text,
                SubText = subText,
                TestId = testId
            };
        }
    }
}
