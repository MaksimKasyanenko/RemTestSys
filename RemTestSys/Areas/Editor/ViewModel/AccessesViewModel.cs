using RemTestSys.Domain.Models;
using System.Collections.Generic;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class AccessesViewModel
    {
        public List<AccessToTestForAll> AccessesForAll { get; set; }
        public List<AccessToTestForGroup> AccessesForGroups { get; set; }
        public List<AccessToTestForStudent> AccessesForStudents { get; set; }
    }
}
