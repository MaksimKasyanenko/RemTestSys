namespace RemTestSys.Domain.Models
{
    public class QuestionInSession
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public Question Question { get; set; }
        public int SerialNumber { get; set; }
    }
}
