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




        public static QuestionWithTextAnswerViewModel CreateForTextAnswer(Question question)
        {
            var vm = new QuestionWithTextAnswerViewModel
            {
                Text = question.Text,
                SubText = question.SubText,
                TestId = question.TestId,
                RightText = question.Answer.RightText,
                CaseMatters = ((TextAnswer)question.Answer).CaseMatters
            };
            return vm;
        }
    }
}
