using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class QuestionViewModel
    {
        public int TestId { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }




        public static QuestionWithTextAnswerViewModel CreateForTextAnswer(Question question)
        {
            return new QuestionWithTextAnswerViewModel
            {
                Text = question.Text,
                SubText = question.SubText,
                TestId = question.TestId,
                RightText = question.Answer.RightText,
                CaseMatters = ((TextAnswer)question.Answer).CaseMatters
            };
        }

        public static QuestionWithOneOfFourVariantsAnswerViewModel CreateForOneOfFourAnswer(Question question)
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(((OneOfFourVariantsAnswer)question.Answer).SerializedFakes);
            return new QuestionWithOneOfFourVariantsAnswerViewModel {
                TestId=question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                RightVariant=question.Answer.RightText,
                Fake1 = fakes[0],
                Fake2 = fakes[1],
                Fake3 = fakes[2]
            };
        }

        public static QuestionWithSomeVariantsAnswerViewModel CreateForSomeVariantsAnswer(Question question)
        {
            SomeVariantsAnswer answer = (SomeVariantsAnswer)question.Answer;
            string[] rights = JsonSerializer.Deserialize<string[]>(answer.SerializedRightAnswers);
            string[] fakes = JsonSerializer.Deserialize<string[]>(answer.SerializedFakes);
            return new QuestionWithSomeVariantsAnswerViewModel {
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                RightVariants = rights,
                FakeVariants = fakes
            };
        }

        internal static QuestionWithSequenceAnswerViewModel CreateForSequenceAnswer(Question question)
        {
            SequenceAnswer answer = (SequenceAnswer)question.Answer;
            string[] sequence = JsonSerializer.Deserialize<string[]>(answer.SerializedSequence);
            return new QuestionWithSequenceAnswerViewModel {
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Sequence = sequence
            };
        }

        internal static QuestionWithConnectedPairsAnswerViewModel CreateForConnectedPairAnswer(Question question)
        {
            ConnectedPairsAnswer answer = (ConnectedPairsAnswer)question.Answer;
            ConnectedPairsAnswer.Pair[] pairs = JsonSerializer.Deserialize<ConnectedPairsAnswer.Pair[]>(answer.SerializedPairs);
            return new QuestionWithConnectedPairsAnswerViewModel {
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                LeftList = pairs.Select(p => p.Value1).ToArray(),
                RightList = pairs.Select(p => p.Value2).ToArray()
            };
        }
    }
}
