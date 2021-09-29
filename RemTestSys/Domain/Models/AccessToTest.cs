namespace RemTestSys.Domain.Models
{
    public class AccessToTest
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public bool EveryBody { get; set; }
        public Test Test { get; set; }
    }
}
