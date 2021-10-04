using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class SomeVariantsAnswer : AnswerBase
    {
        public string SerializedRightAnswers { get; set; }
        public string SerializedFakes { get; set; }

        public override string[] GetAddition()
        {
            throw new NotImplementedException();
        }

        public override bool IsMatch(string[] data)
        {
            throw new NotImplementedException();
        }

        public void SetFakes(params string[] fakes)
        {
            throw new NotImplementedException();
        }

        public void SetRightAnswers(params string[] rightAnswers)
        {
            throw new NotImplementedException();
        }
    }
}
