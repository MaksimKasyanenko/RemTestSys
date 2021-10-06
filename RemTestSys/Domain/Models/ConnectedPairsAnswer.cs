using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class ConnectedPairsAnswer : AnswerBase
    {
        public string SerializedPairs { get; set; }

        public override string[] GetAdditiveData()
        {
            throw new NotImplementedException();
        }

        public override bool IsMatch(string[] data)
        {
            throw new NotImplementedException();
        }

        public void SetAdditiveData(string[] leftList, string[] rightList)
        {
            throw new NotImplementedException();
        }
    }
}
