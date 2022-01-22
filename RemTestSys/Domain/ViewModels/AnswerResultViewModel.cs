using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.ViewModels
{
    public class AnswerResultViewModel
    {
        public bool IsRight { get; internal set; }
        public string RightText { get; internal set; }
    }
}
