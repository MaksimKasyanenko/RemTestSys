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
                if (value == null) throw new InvalidOperationException("Property cannot be setted as NULL");
                var pairs = JsonSerializer.Deserialize<Pair[]>(value);
                SetPairs(pairs);
            }
        }
        private string _serializedPairs;

        public void SetPairs(Pair[] pairs)
        {
            if (pairs == null) throw new InvalidOperationException("Pairs cannot be NULL");
            if (pairs.Length < 1) throw new InvalidOperationException("Pairs count must be over then 1");
            _serializedPairs = JsonSerializer.Serialize(pairs);
        }

        public override string[] GetAdditiveData()
        {
            throw new NotImplementedException();
        }

        public override bool IsMatch(string[] data)
        {
            throw new NotImplementedException();
        }

        public struct Pair
        {
            public string Value1 { get; }
            public string Value2 { get; }
            public Pair(string v1, string v2)
            {
                if (v1 == null || v2 == null) throw new ArgumentNullException("Values of the Pair cannot be NULL");
                Value1 = v1;
                Value2 = v2;
            }
        }
    }
}
