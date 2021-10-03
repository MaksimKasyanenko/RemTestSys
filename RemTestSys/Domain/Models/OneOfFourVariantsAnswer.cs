using System;
using System.Text.Json;

namespace RemTestSys.Domain.Models
{
    public class OneOfFourVariantsAnswer : AnswerBase
    {
        public string SerializedFakes { get; set; }
        public void SetFakes(string f1, string f2, string f3)
        {
            string[] fakes = {f1,f2,f3};
            SerializedFakes = JsonSerializer.Serialize(fakes);
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
            for(int i=0; i<mixed.Length; i++)
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
