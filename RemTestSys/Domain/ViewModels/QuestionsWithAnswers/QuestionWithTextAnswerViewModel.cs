namespace RemTestSys.Domain.ViewModels.QuestionsWithAnswers
{
    public class QuestionWithTextAnswerViewModel:QuestionWithAnswerViewModel
    {
        public string RightText { get; set; }
        public bool CaseMatters { get; set; }
    }
}
