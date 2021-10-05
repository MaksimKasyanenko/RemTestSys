using System;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace RemTestSys.Domain.Models
{
    public class SomeVariantsAnswer : AnswerBase
    {
        public override string RightText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var t in JsonSerializer.Deserialize<string[]>(SerializedRightAnswers))
                {
                    sb.Append($"{t}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                return sb.ToString();
            }
            set { }
        }
        public string SerializedRightAnswers
        {
            get
            {
                return serRightAns;
            }
            set
            {
                if (value == null) throw new InvalidOperationException("SerializedRightAnswers property cannot be setted NULL");
                string[] temp = JsonSerializer.Deserialize<string[]>(value);
                SetRightAnswers(temp);
            }
        }
        public string SerializedFakes
        {
            get
            {
                return serFakes;
            }
            set
            {
                if (value == null) throw new InvalidOperationException("SerializedFakes property cannot be setted NULL");
                var temp = JsonSerializer.Deserialize<string[]>(value);
                SetFakes(temp);
            }
        }
        private string serRightAns;
        private string serFakes;

        public override string[] GetAddition()
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(SerializedFakes);
            string[] rightAnswers = JsonSerializer.Deserialize<string[]>(SerializedRightAnswers);
            string[] all = new string[fakes.Length+rightAnswers.Length];
            Array.Copy(fakes, 0, all, 0, fakes.Length);
            Array.Copy(rightAnswers, 0, all, fakes.Length, rightAnswers.Length);
            RandomSequence rnd = new RandomSequence(0, all.Length);
            string[] res = new string[all.Length];
            for(int i = 0; i < res.Length; i++)
            {
                res[i] = all[rnd.GetNext()];
            }
            return res;
        }

        public override bool IsMatch(string[] data)
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(SerializedFakes);
            string[] rightAnswers = JsonSerializer.Deserialize<string[]>(SerializedRightAnswers);

            foreach(var t in data)
            {
                if (fakes.Any(f => f == t)) return false;
            }
            foreach(var t in rightAnswers)
            {
                if (!data.Contains(t)) return false;
            }
            return true;
        }

        public void SetFakes(params string[] fakes)
        {
            if (fakes == null) throw new InvalidOperationException("Passed args cannot be NULL");
            if(fakes.Contains(null)) throw new InvalidOperationException("Fakes cannot contain NULL");
            serFakes = JsonSerializer.Serialize(fakes);
        }

        public void SetRightAnswers(params string[] rightAnswers)
        {
            if (rightAnswers == null) throw new InvalidOperationException("Passed args cannot be NULL");
            if (rightAnswers.Length<1) throw new InvalidOperationException("Right Answers must contain at least 1 answer");
            if (rightAnswers.Contains(null)) throw new InvalidOperationException("Right Answers cannot contain NULL");
            serRightAns = JsonSerializer.Serialize(rightAnswers);
        }
    }
}
