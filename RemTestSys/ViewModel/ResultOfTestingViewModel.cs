using System;

namespace RemTestSys.ViewModel
{
    public class ResultOfTestingViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string Mark { get; set; }
        public DateTime PassedAt { get; set; }
    }
}
