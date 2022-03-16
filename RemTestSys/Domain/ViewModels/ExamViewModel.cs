namespace RemTestSys.Domain.ViewModels
{
    public class ExamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
        public int Duration { get; set; }
        public string MaxMark { get; set; }
    }
}