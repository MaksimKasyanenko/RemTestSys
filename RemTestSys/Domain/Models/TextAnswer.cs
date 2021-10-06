using System;

namespace RemTestSys.Domain.Models
{
    public class TextAnswer : AnswerBase
    {
        public bool CaseMatters { get; set; }
        public override string[] GetAdditiveData()
        {
            return null;
        }

        public override bool IsMatch(string[] data)
        {
            if (data == null) return false;
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
            if (Array.IndexOf(correspTableEng, a) < 0 && Array.IndexOf(correspTableUkr, a) < 0) return false;//in order to prevent -1 == -1 bellow...
            if (Array.IndexOf(correspTableEng, a) == Array.IndexOf(correspTableUkr, b)) return true;
            if (Array.IndexOf(correspTableEng, b) == Array.IndexOf(correspTableUkr, a)) return true;
            return false;
        }

        private char[] correspTableEng = {'a','c','e','i','l','o','p','x','y','A','B','C','E','H','I','K','M','O','P','T','X'};
        private char[] correspTableUkr = {'а','с','е','і','і','о','р','х','у','А','В','С','Е','Н','І','К','М','О','Р','Т','Х'};
    }
}
