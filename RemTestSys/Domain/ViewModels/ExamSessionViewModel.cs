namespace RemTestSys.Domain.ViewModels
{
    public class ExamSessionViewModel
    {
        public string TestName { get; set; }
        public int QuestionsCount { get; set; }
        public int QuestionNum { get; internal set; }
        public int TimeLeft { get; internal set; }
        public string QuestionText { get; internal set; }
        public string QuestionSubText { get; internal set; }
        public double QuestionCost{get; internal set;}
        public string AnswerType { get; internal set; }
        public string[] Addition { get; internal set; }
        public bool Finished { get; internal set; }
        public int SessionId { get; internal set; }
        public int? ResultId { get; internal set; }
    }
}