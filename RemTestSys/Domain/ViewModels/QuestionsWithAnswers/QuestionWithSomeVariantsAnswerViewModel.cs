namespace RemTestSys.Domain.ViewModels.QuestionsWithAnswers
{
    public class QuestionWithSomeVariantsAnswerViewModel:QuestionWithAnswerViewModel
    {
        public string[] RightVariants { get; set; }
        public string[] FakeVariants { get; set; }
    }
}
