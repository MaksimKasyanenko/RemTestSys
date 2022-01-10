using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces{
    public interface IExamService{
        Task<IEnumerable<ExamInfo>> GetAvailableExamsFor(int studentId);
        Task<IEnumerable<double>> GetResultsFor(int studentId, int testId);
    }
}