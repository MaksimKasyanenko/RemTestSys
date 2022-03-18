namespace RemTestSys.Domain.ViewModels.QuestionsWithAnswers
{
    public class QuestionWithConnectedPairsAnswerViewModel:QuestionWithAnswerViewModel
    {
        public string[] LeftList { get; set; }
        public string[] RightList { get; set; }
    }
}
