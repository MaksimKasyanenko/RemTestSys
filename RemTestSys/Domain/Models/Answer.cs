using System;

namespace RemTestSys.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public Types Type
        {
            get
            {
                if (answerMatcher == null || additionLinker == null) throw new InvalidOperationException("Property 'Type' mustn't be read before it have been set");
                return type;
            }
            set
            {
                answerMatcher = AnswerMatcher.Create(value);
                additionLinker = AdditionLinker.Create(value);
                type = value;
            }
        }
        private AnswerMatcher answerMatcher;
        private AdditionLinker additionLinker;
        private Types type;
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
        public string[] GetAddition()
        {
            return additionLinker.Link(this);
        }
        public bool IsMatch(string[] data)
        {
            return answerMatcher.Match(data);
        }



        public enum Types { Text, OneVariant, MultipleVariant, Chain, Conformity }





        private abstract class AnswerMatcher
        {

        }
    }
}
