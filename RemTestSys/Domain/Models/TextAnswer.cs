using System;

namespace RemTestSys.Domain.Models
{
    public class TextAnswer : AnswerBase
    {
        public override string[] GetAddition()
        {
            return null;
        }

        public override bool IsMatch(string[] data)
        {
            if (data == null) throw new InvalidOperationException("Answer don't support matching with NULL");
            if (data.Length != 1) return false;
            string inpt = data[0].Trim().Replace(" ", "");
            string exp = RightText.Trim().Replace(" ", "");
            if (inpt.Length != exp.Length) return false;
            if (!CaseMatters)
            {
                inpt = inpt.ToLower();
                exp = exp.ToLower();
            }
            for(int i=0; i < inpt.Length; i++)
            {
                if (!EqualChars(inpt[i], exp[i])) return false;
            }
            return true;
        }

        private bool EqualChars(char a, char b)
        {
            if (a == b) return true;
            if (Array.IndexOf(correspTableEng, a) < 0 || Array.IndexOf(correspTableUkr, a) < 0) return false;//in order to prevent -1 == -1 bellow...
            if (Array.IndexOf(correspTableEng, a) == Array.IndexOf(correspTableUkr, b)) return true;
            if (Array.IndexOf(correspTableEng, b) == Array.IndexOf(correspTableUkr, a)) return true;
            return false;
        }

        private char[] correspTableEng = {'a','b','c','e','h','i','g','k','l','m','n','o','p','t','x','y'};
        private char[] correspTableUkr = {'а','в','с','е','н','і','д','к','і','т','п','о','р','т','х','у'};
    }
}
