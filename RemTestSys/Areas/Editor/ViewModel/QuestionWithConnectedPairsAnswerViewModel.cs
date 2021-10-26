using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionWithConnectedPairsAnswerViewModel:QuestionViewModel
    {
        public string[] LeftList { get; set; }
        public string[] RightList { get; set; }
    }
}
