using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface IStudentService
    {
        Task<Student> FindStudent(string logId);
        Task<List<Exam>> GetExamsListForStudent(int id);
        Task<Test> GetTestForStudent(int id1, int id2);
    }
}