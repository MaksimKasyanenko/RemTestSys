using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface IExamsDbContext
    {
        Task<IEnumerable<Exam>> GetExamsListForStudent(int studentId);
    }
}