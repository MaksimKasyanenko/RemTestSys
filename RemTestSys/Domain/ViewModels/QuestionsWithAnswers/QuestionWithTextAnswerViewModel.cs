namespace RemTestSys.Domain.ViewModels.QuestionsWithAnswers
{
    public class QuestionWithTextAnswerViewModel:QuestionViewModel
    {
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
    }
}
