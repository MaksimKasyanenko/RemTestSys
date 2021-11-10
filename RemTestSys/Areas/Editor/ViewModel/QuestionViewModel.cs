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

        internal static string CreateForSequenceAnswer(Question question)
        {
            throw new NotImplementedException();
        }

        internal static string CreateForConnectedPairAnswer(Question question)
        {
            throw new NotImplementedException();
        }
    }
}
