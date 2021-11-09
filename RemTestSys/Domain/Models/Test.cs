using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RemTestSys.Domain.Models
{
    public sealed class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionsCount { get; set; }
        public int Duration { get; set; }
        public double ScoresPerRightAnswer { get; set; }
        [JsonIgnore]
        public IEnumerable<Question> Questions { get; set; }
    }
}
