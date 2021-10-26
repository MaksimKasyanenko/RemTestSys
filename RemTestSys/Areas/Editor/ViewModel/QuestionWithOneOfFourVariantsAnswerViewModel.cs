using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionWithOneOfFourVariantsAnswerViewModel:QuestionViewModel
    {
        public string RightVariant { get; set; }
        public string Fake1 { get; set; }
        public string Fake2 { get; set; }
        public string Fake3 { get; set; }
    }
}
