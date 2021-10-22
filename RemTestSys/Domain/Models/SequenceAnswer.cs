using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class SequenceAnswer : Answer
    {
        public override string RightText {
            get
            {
                StringBuilder sb = new StringBuilder();
                string[] sequence = JsonSerializer.Deserialize<string[]>(SerializedSequence);
                foreach(var el in sequence)
                {
                    sb.Append(el);
                    sb.Append(", ");
                }
                sb.Remove(sb.Length-2, 2);
                return sb.ToString();
            }
            set { }
        }
        public string SerializedSequence
        {
            get
            {
                return _serializedSequence;
            }
            set
            {
                if (value == null) throw new InvalidOperationException("Setting NULL to SerializedSequence");
                SetSequence(JsonSerializer.Deserialize<string[]>(value));
            }
        }
        private string _serializedSequence;

        public override string[] GetAdditiveData()
        {
            string[] rightSequence = JsonSerializer.Deserialize<string[]>(SerializedSequence);
            RandomSequence randomNumSeq = new RandomSequence(0, rightSequence.Length);
            string[] res = new string[rightSequence.Length];

            for(int i = 0; i < res.Length; i++)
            {
                res[i] = rightSequence[randomNumSeq.GetNext()];
            }
            return res;
        }

        public override bool IsMatch(string[] data)
        {
            string[] rightSequence = JsonSerializer.Deserialize<string[]>(SerializedSequence);
            if (data == null) return false;
            if (data.Length != rightSequence.Length) return false;
            for(int i = 0; i < data.Length; i++)
            {
                if (data[i] != rightSequence[i]) return false;
            }
            return true;
        }

        public void SetSequence(string[] sequence)
        {
            if (sequence == null) throw new InvalidOperationException("Sequence mustn't be NULL");
            if (sequence.Length < 2) throw new InvalidOperationException("Sequence length must be over 1");
            if (sequence.Contains(null)) throw new InvalidOperationException("Sequence mustn't contain any null");

            _serializedSequence = JsonSerializer.Serialize(sequence);
        }
    }
}
