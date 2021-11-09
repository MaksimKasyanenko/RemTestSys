using System;
using System.Text.Json.Serialization;

namespace RemTestSys.Domain.Models
{
    public abstract class Answer
    {
        public int Id { get; set; }
        public virtual string RightText { get; set; }
        public abstract string[] GetAdditiveData();
        public abstract bool IsMatch(string[] data);
        public int QuestionId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }

        public static TextAnswer CreateTextAnswer(string text, bool caseMatters)
        {
            return new TextAnswer {
                RightText=text,
                CaseMatters = caseMatters
            };
        }
        public static OneOfFourVariantsAnswer CreateOneOfFourVariantsAnswer(string rightVariant, string fake1, string fake2, string fake3)
        {
            OneOfFourVariantsAnswer answer = new OneOfFourVariantsAnswer();
            answer.RightText = rightVariant;
            answer.SetFakes(fake1, fake2, fake3);
            return answer;
        }
        public static SomeVariantsAnswer CreateSomeVariantsAnswer(string[] rightVariants, string[] fakeVariants)
        {
            SomeVariantsAnswer answer = new SomeVariantsAnswer();
            answer.SetRightAnswers(rightVariants);
            answer.SetFakes(fakeVariants);
            return answer;
        }
        public static SequenceAnswer CreateSequenceAnswer(string[] sequence)
        {
        	SequenceAnswer answer = new SequenceAnswer();
        	answer.SetSequence(sequence);
        	return answer;
        }
        public static ConnectedPairsAnswer CreateConnectedPairsAnswer(string[] leftList, string[] rightList)
        {
        	if(leftList.Length!=rightList.Length)throw new InvalidOperationException();
        	ConnectedPairsAnswer.Pair[] pairs = new ConnectedPairsAnswer.Pair[leftList.Length];
        	for(int i=0;i<pairs.Length;i++){
        		pairs[i]=new ConnectedPairsAnswer.Pair(leftList[i],rightList[i]);
        	}
        	ConnectedPairsAnswer answer = new ConnectedPairsAnswer();
        	answer.SetPairs(pairs);
        	return answer;
        }
    }
}
