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
        public int QuestionId{get;set;}
        public string Text { get; set; }
        public string SubText { get; set; }
        public double Cast{get;set;}




        public static QuestionWithTextAnswerViewModel CreateForTextAnswer(Question question)
        {
            return new QuestionWithTextAnswerViewModel
            {
                QuestionId=question.Id,
                Text = question.Text,
                SubText = question.SubText,
                Cast=question.Cast,
                TestId = question.TestId,
                RightText = question.Answer.RightText,
                CaseMatters = ((TextAnswer)question.Answer).CaseMatters
            };
        }

        public static QuestionWithOneOfFourVariantsAnswerViewModel CreateForOneOfFourAnswer(Question question)
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(((OneOfFourVariantsAnswer)question.Answer).SerializedFakes);
            return new QuestionWithOneOfFourVariantsAnswerViewModel {
                QuestionId=question.Id,
                TestId=question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cast=question.Cast,
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
                QuestionId=question.Id,
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cast=question.Cast,
                RightVariants = rights,
                FakeVariants = fakes
            };
        }

        internal static QuestionWithSequenceAnswerViewModel CreateForSequenceAnswer(Question question)
        {
            SequenceAnswer answer = (SequenceAnswer)question.Answer;
            string[] sequence = JsonSerializer.Deserialize<string[]>(answer.SerializedSequence);
            return new QuestionWithSequenceAnswerViewModel {
                QuestionId=question.Id,
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cast=question.Cast,
                Sequence = sequence
            };
        }

        internal static QuestionWithConnectedPairsAnswerViewModel CreateForConnectedPairAnswer(Question question)
        {
            ConnectedPairsAnswer answer = (ConnectedPairsAnswer)question.Answer;
            ConnectedPairsAnswer.Pair[] pairs = JsonSerializer.Deserialize<ConnectedPairsAnswer.Pair[]>(answer.SerializedPairs);
            return new QuestionWithConnectedPairsAnswerViewModel {
                QuestionId=question.Id,
                TestId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cast=question.Cast,
                LeftList = pairs.Select(p => p.Value1).ToArray(),
                RightList = pairs.Select(p => p.Value2).ToArray()
            };
        }
    }
}
