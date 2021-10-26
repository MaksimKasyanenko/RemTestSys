using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionWithSomeVariantsAnswerViewModel:QuestionViewModel
    {
        public string[] RightVariants { get; set; }
        public string[] FakeVariants { get; set; }
    }
}
