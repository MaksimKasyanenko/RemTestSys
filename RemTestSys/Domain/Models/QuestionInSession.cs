namespace RemTestSys.Domain.Models
{
    public class QuestionInSession
    {
        public int Id { get; set; }
        public Session Session { get; set; }
        public Question Question { get; set; }
        public int SerialNumber { get; set; }
    }
}
