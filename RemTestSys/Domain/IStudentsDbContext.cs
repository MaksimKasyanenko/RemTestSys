using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface IStudentsDbContext
    {
        Task<Student> FindStudent(string logId);
        Task<IEnumerable<AccessToTest>> GetAccessListToTestsForStudent(int studentId);
    }
}