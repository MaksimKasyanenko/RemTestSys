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
        public AnswerTypes AnswerType { get; set; }
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
        public string RightVariant { get; set; }
        public string[] Fakes { get; set; }
        public string[] SomeRightVariants { get; set; }
        public string[] SomeFakeVariants { get; set; }

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
                case AnswerTypes.OneOfFourVariants: res = Answer.Create(RightVariant, Fakes[0], Fakes[1],Fakes[2]);break;
                case AnswerTypes.SomeVariants:res = Answer.Create(SomeRightVariants, SomeFakeVariants);break;
            }
            return res??throw new NullReferenceException("Suitable type for answer is not defined here");
        }

        public enum AnswerTypes { Text, OneOfFourVariants, SomeVariants }
    }
}
