using System.Linq;
using System.Collections.Generic;

namespace RemTestSys.Domain.Models
{
    public sealed class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public IEnumerable<MapPart> MapParts{get;set;}
        public IEnumerable<Question> Questions { get; set; }
        [NotMapped]
        public int QuestionsCount
        {
            get
            {
                if(MapParts==null)return 0;
                return MapParts.Agregate(0, (prev, next)=>prev+next.QuestionCount);
            }
        }
        [NotMapped]
        public int ScoreSum
        {
            get
            {
                if(MapParts==null)return 0;
                return MapParts.Agregate(0, (prev,next)=>prev+next.QuestionCount*next.Cast);
            }
        }

        public class MapPart
        {
            public int Id{get;set;}
            public int QuestionCount{get;set;}
            public double Cast{get;set;}
            public int TestId{get;set;}
            public Test Test{get;set;}
        }
    }
}
