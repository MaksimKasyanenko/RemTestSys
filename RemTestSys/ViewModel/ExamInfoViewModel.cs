using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using System;

namespace RemTestSys.ViewModel
{
    public class ExamInfoViewModel
    {
        public ExamInfoViewModel(Exam exam)
        {
            _exam = exam;
        }
        private readonly Exam _exam;
        public int TestId { get => _exam.Test.Id; }
        public string TestName { get => _exam.Test.Name; }
        public string TestDescription { get => _exam.Test.Description; }
        public ExamStatus Status { get => _exam.Status; }
        public int CountOfQuestions { get => _exam.Test.QuestionsCount; }
        public int Duration { get => _exam.Test.Duration; }
        public DateTime Deadline { get => _exam.Deadline; }
    }
}
