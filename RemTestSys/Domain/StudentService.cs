using RemTestSys.Domain.Exceptions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public class StudentService : IStudentService
    {
        public StudentService(IStudentsDbContext studentsDbContext, IExamsDbContext examsDbContext)
        {
            _studentsDbContext = studentsDbContext ?? throw new ArgumentNullException(nameof(IStudentsDbContext));
            _examsDbContext = examsDbContext ?? throw new ArgumentNullException(nameof(IExamsDbContext));
        }

        private readonly IStudentsDbContext _studentsDbContext;
        private readonly IExamsDbContext _examsDbContext;

        public async Task<Student> GetStudent(string studentLogId)
        {
            Student student = await _studentsDbContext.FindStudent(studentLogId);
            if (student == null) throw new NonExistException($"Student for specified LogId({studentLogId}) do not exists");
            return student;
        }

        public async Task<IEnumerable<Exam>> GetExamsForStudent(int studentId)
        {
            return (await _examsDbContext.GetExams(ex => ex.AssignedTo.Id == studentId))
                                        .Where(e => e.Status == ExamStatus.NotPassed || !e.IsStrict)
                                        .Select(e => e);
        }

        public async Task<Test> GetTestForStudent(int testId, int studentId)
        {
            AccessToTest access = (await _studentsDbContext.GetAccessesToTests(at => at.Test.Id == testId && at.Student.Id == studentId)).Single();
            if (access == null) throw new DataAccessException($"Student (id:{studentId}) haven't got access to test (id:{testId})");
            return access.Test;
        }
    }
}
