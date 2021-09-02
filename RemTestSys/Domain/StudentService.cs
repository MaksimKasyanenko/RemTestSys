using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public class StudentService : IStudentService
    {
        public StudentService(IStudentsDbContext studentsDbContext, IExamsDbContext examsDbContext, ITestsDbContext testsDbContext)
        {
            _studentsDbContext = studentsDbContext ?? throw new ArgumentNullException(nameof(IStudentsDbContext));
            _examsDbContext = examsDbContext ?? throw new ArgumentNullException(nameof(IExamsDbContext));
            _testsDbContext = testsDbContext ?? throw new ArgumentNullException(nameof(ITestsDbContext));
        }

        private readonly IStudentsDbContext _studentsDbContext;
        private readonly IExamsDbContext _examsDbContext;
        private readonly ITestsDbContext _testsDbContext;

        public async Task<Student> FindStudent(string studentLogId)
        {
            return await _studentsDbContext.FindStudent(studentLogId);
        }

        public async Task<List<Exam>> GetExamsListForStudent(int studentId)
        {
            var queryExams = from exam in await _examsDbContext.GetExamsListForStudent(studentId)
                             where exam.Status == ExamStatus.NotPassed
                                   || !exam.IsStrict
                                   || exam.Deadline > DateTime.Now
                             select exam;
            return queryExams.ToList();
        }

        public async Task<Test> GetTestForStudent(int testId, int studentId)
        {
            AccessToTest access = (await _studentsDbContext.GetAccessListToTestsForStudent(studentId)).FirstOrDefault(acc => acc.Test.Id == testId);
            if (access == null) throw new NullReferenceException(nameof(AccessToTest));
            Test test = await _testsDbContext.GetTest(testId) ?? throw new NullReferenceException(nameof(Test));
            return test;
        }
    }
}
