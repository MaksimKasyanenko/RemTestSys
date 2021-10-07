using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class ConnectedPairsAnswer : AnswerBase
    {
        public string SerializedPairs
        {
            get
            {
                return _serializedPairs;
            }
            set
            {
                if (value == null) throw new InvalidOperationException("The property cannot be setted as NULL");
                SetAdditiveData(JsonSerializer.Deserialize<string[]>(value));
            }
        }
        private string _serializedPairs;

        public override string[] GetAdditiveData()
        {
            string[] temp =  JsonSerializer.Deserialize<string[]>(_serializedPairs);
            string[] res = new string[temp.Length];
            RandomSequence rnd = new RandomSequence(0, res.Length/2);
            for(int i = 0; i < res.Length / 2; i++)
            {
                int r = rnd.GetNext();
                res[i*2] = temp[r*2];
                res[i*2+1] = temp[r*2+1];
            }
            return res;
        }

        public override bool IsMatch(string[] data)
        {
            throw new NotImplementedException();
        }

        public void SetAdditiveData(string[] leftList, string[] rightList)
        {
            if (leftList == null || rightList == null) throw new InvalidOperationException("Both of lists must be not NULL");
            if (leftList.Length != rightList.Length) throw new InvalidOperationException("Lengthes of lists must be equal");
            string[] unitedData = new string[leftList.Length+rightList.Length];
            int unitedIndex = 0;
            for(int i=0; i < leftList.Length; i++)
            {
                unitedData[unitedIndex++] = leftList[i];
                unitedData[unitedIndex++] = rightList[i];
            }
            SetAdditiveData(unitedData);
        }
        private void SetAdditiveData(string[] allData)
        {
            if (allData.Length % 2 != 0) throw new InvalidOperationException("Count of array elements must be even");
            if (allData.Length < 2) throw new InvalidOperationException("Need at least 1 pair");
            if (allData.Contains(null)) throw new InvalidOperationException("The lists cannot contain any NULL element");
            _serializedPairs = JsonSerializer.Serialize(allData);
        }
    }
}
