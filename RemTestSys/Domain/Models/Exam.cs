using System;

namespace RemTestSys.Domain.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public Test Test { get; set; }
        public Student AssignedTo { get; set; }
        public ExamStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsStrict { get; set; }
    }
}
