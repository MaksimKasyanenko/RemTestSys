using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces{
    public interface IExamService{
        Task<IEnumerable<ExamViewModel>> GetAvailableExamsForAsync(int studentId);
        Task<IEnumerable<ExamResultViewModel>> GetResultsForAsync(int studentId);
        Task<bool> HasAccessTo(int sudentId, int examId);
    }
}