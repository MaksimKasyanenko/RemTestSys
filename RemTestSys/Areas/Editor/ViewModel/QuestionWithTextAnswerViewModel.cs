using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionWithTextAnswerViewModel:QuestionViewModel
    {
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
    }
}
