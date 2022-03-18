using System.Text.Json;
using System.Linq;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels.QuestionsWithAnswers;
namespace RemTestSys.Domain.ViewModels;

public class QuestionViewModel
{
    public int Id{get;set;}
    public int ExamId{get;set;}
    public string Text{get;set;}
    public string SubText{get;set;}
    public string RightAnswer{get;set;}
    public double Cost{get;set;}

    public static QuestionWithTextAnswerViewModel CreateForTextAnswer(Question question)
        {
            return new QuestionWithTextAnswerViewModel
            {
                Id=question.Id,
                Text = question.Text,
                SubText = question.SubText,
                Cost=question.Cast,
                ExamId = question.TestId,
                RightText = question.Answer.RightText,
                CaseMatters = ((TextAnswer)question.Answer).CaseMatters
            };
        }

        public static QuestionWithOneOfFourVariantsAnswerViewModel CreateForOneOfFourAnswer(Question question)
        {
            string[] fakes = JsonSerializer.Deserialize<string[]>(((OneOfFourVariantsAnswer)question.Answer).SerializedFakes);
            return new QuestionWithOneOfFourVariantsAnswerViewModel {
                Id=question.Id,
                ExamId=question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cost=question.Cast,
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
                Id=question.Id,
                ExamId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cost=question.Cast,
                RightVariants = rights,
                FakeVariants = fakes
            };
        }

        internal static QuestionWithSequenceAnswerViewModel CreateForSequenceAnswer(Question question)
        {
            SequenceAnswer answer = (SequenceAnswer)question.Answer;
            string[] sequence = JsonSerializer.Deserialize<string[]>(answer.SerializedSequence);
            return new QuestionWithSequenceAnswerViewModel {
                Id=question.Id,
                ExamId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cost=question.Cast,
                Sequence = sequence
            };
        }

        internal static QuestionWithConnectedPairsAnswerViewModel CreateForConnectedPairAnswer(Question question)
        {
            ConnectedPairsAnswer answer = (ConnectedPairsAnswer)question.Answer;
            ConnectedPairsAnswer.Pair[] pairs = JsonSerializer.Deserialize<ConnectedPairsAnswer.Pair[]>(answer.SerializedPairs);
            return new QuestionWithConnectedPairsAnswerViewModel {
                Id=question.Id,
                ExamId = question.TestId,
                Text = question.Text,
                SubText = question.SubText,
                Cost=question.Cast,
                LeftList = pairs.Select(p => p.Value1).ToArray(),
                RightList = pairs.Select(p => p.Value2).ToArray()
            };
        }
}