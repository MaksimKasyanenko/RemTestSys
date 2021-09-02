using System;

namespace RemTestSys.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
        public object Addition => null;

        public bool IsMatch(object data)
        {
            if (data.GetType() == typeof(string)) return false;
            string text = ((string)data).Trim();
            return (text == RightText.Trim()) || (!CaseMatters && text.Trim().ToLower() == RightText.Trim().ToLower());
        }
    }
}
