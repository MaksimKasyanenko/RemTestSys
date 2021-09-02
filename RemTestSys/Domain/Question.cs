namespace RemTestSys.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string SubText { get; set; }
        public Answer Answer { get; set; }
        public Test ContainedIn { get; set; }
    }
}
