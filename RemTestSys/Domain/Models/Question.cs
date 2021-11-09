using System;
using System.Text.Json.Serialization;

namespace RemTestSys.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public int TestId { get; set; }
        [JsonIgnore]
        public Test Test { get; set; }
        public Answer Answer { get; set; }

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
