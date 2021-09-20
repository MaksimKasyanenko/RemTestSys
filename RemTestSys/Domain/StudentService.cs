using Microsoft.EntityFrameworkCore;
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
        public StudentService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        private readonly AppDbContext _appDbContext;

        public async Task<Student> GetStudent(string studentLogId)
        {
            Student student = await _appDbContext.Students.Where(st => st.LogId == studentLogId).Include(st=>st.Group).FirstOrDefaultAsync();
            if (student == null) throw new NotExistException($"Student for specified LogId({studentLogId}) do not exists");
            return student;
        }

        public async Task<IEnumerable<Exam>> GetExamsForStudent(int studentId)
        {
            return await _appDbContext.Exams
                                       .Where(ex => ex.AssignedTo.Id == studentId)
                                       .Where(e => e.Status == ExamStatus.NotPassed || !e.IsStrict)
                                       .ToArrayAsync();
        }

        public async Task<Test> GetTestForStudent(int testId, int studentId)
        {
            AccessToTest access = await _appDbContext.AccessesToTest
                                                     .Where(at => at.Test.Id == testId && at.Student.Id == studentId)
                                                     .Include(ac=>ac.Test)
                                                     .ThenInclude(a=>a.Questions)
                                                     .FirstOrDefaultAsync();
            if (access == null) throw new DataAccessException($"Student (id:{studentId}) haven't got access to test (id:{testId})");
            return access.Test;
        }

        public async Task<bool> StudentExists(string logId)
        {
            try
            {
                Student student = await GetStudent(logId);
                return true;
            }
            catch (NotExistException)
            {
                return false;
            }
        }
    }
}
