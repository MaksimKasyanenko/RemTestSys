using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces{
    public interface IExamService{
        Task<IEnumerable<ExamViewModel>> GetAvailableExamsForAsync(int studentId);
        Task<IEnumerable<ExamResultViewModel>> GetResultsForAsync(int studentId);
        Task<bool> HasAccessToAsync(int sudentId, int examId);
        Task<ExamSessionViewModel> ExamineAsync(int studentId, int examId);
        Task<ExamSessionViewModel> GetSessionStateForAsync(int sessionId, int studentId);
        Task<AnswerResultViewModel> AnswerQuestionAsync(int sessionId, AnswerViewModel answer);
        Task<ExamResultViewModel> GetResultForAsync(int resultId, int studentId);
    }
}