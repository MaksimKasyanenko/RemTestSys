using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class ResultOfTesting
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Test Test { get; set; }
        public int Mark { get; set; }
        public DateTime PassedAt { get; set; }

    }
}
