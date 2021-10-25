using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionViewModel
    {
        public int TestId { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public bool IsValid { get => Validate(); }
        public AnswerTypes AnswerType { get; set; }
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }

        public Question GetQuestion()
        {
            return new Question {
                TestId = this.TestId,
                Text = this.Text,
                SubText = this.SubText
            };
        }
        public Answer GetAnswer()
        {
            Answer res=null;
            switch (AnswerType)
            {
                case AnswerTypes.Text: res = Answer.Create(RightText, CaseMatters);break;
            }
            return res??throw new NullReferenceException("Suitable type for answer is not defined here");
        }
        private bool Validate()
        {
            if (TestId < 1) return false;
            if (Text == null || Text == "") return false;
            switch (AnswerType)
            {
                case AnswerTypes.Text: if (RightText == null || RightText == "") return false; break;
            }
            return true;
        }

        public enum AnswerTypes { Text }
    }
}
