using System;
using System.Text.Json;

namespace RemTestSys.Domain.Models
{
    public class OneOfFourVariantsAnswer : AnswerBase
    {
        public string SerializedFakes
        {
            get { return _seriazedFakes; }
            set
            {
                string[] temp = JsonSerializer.Deserialize<string[]>(value);
                if (temp.Length != 3) throw new InvalidOperationException("Json string is wrong");   
                SetFakes(temp[0], temp[1], temp[2]);
            }
        }
        private string _seriazedFakes;
        public void SetFakes(string f1, string f2, string f3)
        {
            if (f1 == null || f2 == null || f3 == null) throw new InvalidOperationException($"Some of parameters equals null");
            string[] fakes = { f1, f2, f3 };
            _seriazedFakes = JsonSerializer.Serialize(fakes);
        }
        public override string[] GetAddition()
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(SerializedFakes);
            string[] res = new string[fakes.Length + 1];
            res[0] = RightText;
            Array.Copy(fakes, 0, res, 1, fakes.Length);
            res = Mix(res);
            return res;
        }
        private string[] Mix(string[] arr)
        {
            var rnd = new RandomSequence(0, arr.Length);
            string[] mixed = new string[arr.Length];
            for (int i = 0; i < mixed.Length; i++)
            {
                mixed[i] = arr[rnd.GetNext()];
            }
            return mixed;
        }

        public override bool IsMatch(string[] data)
        {
            if (data == null) return false;
            if (data.Length != 1) return false;
            if (data[0] != RightText) return false;
            return true;
        }
    }
}
