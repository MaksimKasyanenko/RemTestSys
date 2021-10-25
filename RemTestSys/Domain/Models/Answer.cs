using System;

namespace RemTestSys.Domain.Models
{
    public abstract class Answer
    {
        public int Id { get; set; }
        public virtual string RightText { get; set; }
        public abstract string[] GetAdditiveData();
        public abstract bool IsMatch(string[] data);
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public static Answer Create(string text, bool caseMatters)
        {
            return new TextAnswer {
                RightText=text,
                CaseMatters = caseMatters
            };
        }
        public static Answer Create(string rightVariant, string fake1, string fake2, string fake3)
        {
            OneOfFourVariantsAnswer answer = new OneOfFourVariantsAnswer();
            answer.RightText = rightVariant;
            answer.SetFakes(fake1, fake2, fake3);
            return answer;
        }
        public static Answer Create(string[] rightVariants, string[] fakeVariants)
        {
            SomeVariantsAnswer answer = new SomeVariantsAnswer();
            answer.SetRightAnswers(rightVariants);
            answer.SetFakes(fakeVariants);
            return answer;
        }
    }
}
