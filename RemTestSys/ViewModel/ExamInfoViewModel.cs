using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using System;

namespace RemTestSys.ViewModel
{
    public class ExamInfoViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public ExamStatus Status { get; set; }
        public int CountOfQuestions { get; set; }
        public int Duration { get; set; }
        public DateTime Deadline { get; set; }
    }
}
