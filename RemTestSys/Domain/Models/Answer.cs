using System;

namespace RemTestSys.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
        public string[] GetAddition()
        {
            return null;
        }
        public bool IsMatch(string[] data)
        {
            if (data.Length != 1) return false;
            return (data[0].Trim() == RightText.Trim()) || (!CaseMatters && data[0].Trim().ToLower() == RightText.Trim().ToLower());
        }
    }
}
