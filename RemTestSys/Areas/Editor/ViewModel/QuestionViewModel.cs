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
        public AnswerType AnswerType { get; set; }
        public string RightText { get; set; }
    }
    public enum AnswerType{Text}
}
