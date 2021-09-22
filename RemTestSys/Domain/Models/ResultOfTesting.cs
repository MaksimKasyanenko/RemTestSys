using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class ResultOfTesting
    {
        public int Id { get; internal set; }
        public Student Student { get; internal set; }
        public Test Test { get; internal set; }
        public int Mark { get; internal set; }
    }
}
