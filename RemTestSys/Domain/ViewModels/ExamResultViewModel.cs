using System;

namespace RemTestSys.Domain.ViewModels
{
    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string Mark { get; set; }
        public DateTime PassedAt { get; set; }
    }
}