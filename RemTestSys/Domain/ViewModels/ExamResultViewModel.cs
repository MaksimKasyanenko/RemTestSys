using System;

namespace RemTestSys.Domain.ViewModels
{
    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public int TestId{get;set;}
        public string TestName { get; set; }
        public int StudentId{get;set;}
        public string StudentName{get;set;}
        public int StudentGroupId{get;set;}
        public string StudentGroupName{get;set;}
        public string Mark { get; set; }
        public DateTime PassedAt { get; set; }
    }
}