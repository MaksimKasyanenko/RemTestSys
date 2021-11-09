using System.Text.Json.Serialization;

namespace RemTestSys.Domain.Models
{
    public abstract class AccessToTest
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        [JsonIgnore]
        public Test Test { get; set; }
    }

    public class AccessToTestForStudent : AccessToTest
    {
        public int StudentId { get; set; }
        [JsonIgnore]
        public Student Student { get; set; }
    }

    public class AccessToTestForGroup : AccessToTest
    {
        public int GroupId { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }
    }
    public class AccessToTestForAll : AccessToTest { }
}
