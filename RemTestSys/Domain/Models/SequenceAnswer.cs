using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class SequenceAnswer : AnswerBase
    {
        public string SerializedSequence { get; set; }

        public override string[] GetAdditiveData()
        {
            throw new NotImplementedException();
        }

        public override bool IsMatch(string[] data)
        {
            throw new NotImplementedException();
        }

        public void SetSequence(string[] sequence)
        {
            throw new NotImplementedException();
        }
    }
}
