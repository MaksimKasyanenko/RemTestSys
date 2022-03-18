namespace RemTestSys.Domain.ViewModels.QuestionsWithAnswers
{
    public class QuestionWithSomeVariantsAnswerViewModel:QuestionViewModel
    {
        public string[] RightVariants { get; set; }
        public string[] FakeVariants { get; set; }
    }
}
