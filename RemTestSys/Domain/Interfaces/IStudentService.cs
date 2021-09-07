using RemTestSys.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetStudent(string logId);
        Task<IEnumerable<Exam>> GetExamsForStudent(int id);
        Task<Test> GetTestForStudent(int testId, int studentId);
    }
}