using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
