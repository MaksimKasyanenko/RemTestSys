namespace RemTestSys.Domain.Models
{
    public abstract class AccessToTest
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }

    public class AccessToTestForStudent : AccessToTest
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }

    public class AccessToTestForGroup : AccessToTest
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
    public class AccessToTestForAll : AccessToTest { }
}
