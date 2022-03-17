using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces{
    public interface IExamService{
        Task<List<ExamViewModel>> GetExamsAsync();
        Task<ExamViewModel> FindExamAsync(int id);
        Task CreateExamAsync(ExamViewModel exam);
        Task UpdateExamAsync(ExamViewModel exam);
        Task DeleteExamAsync(int id);
        Task<IEnumerable<ExamViewModel>> GetAvailableExamsForAsync(int studentId);
        Task<IEnumerable<ExamResultViewModel>> GetResultsForAllAsync();
        Task<IEnumerable<ExamResultViewModel>> GetResultsForStudentAsync(int studentId);
        Task<IEnumerable<ExamResultViewModel>> GetResultsOfExamAsync(int examId);
        Task<IEnumerable<ExamResultViewModel>> GetResultsForGroupAsync(int groupId);
        Task RemoveAllResultsAsync();
        Task RemoveResultsForStudentAsync(int studentId);
        Task RemoveResultsForGroupAsync(int groupId);
        Task RemoveResultsOfExamAsync(int examId);
        Task<bool> HasAccessToAsync(int sudentId, int examId);
        Task<ExamSessionViewModel> ExamineAsync(int studentId, int examId);
        Task<ExamSessionViewModel> GetSessionStateForAsync(int sessionId, int studentId);
        Task<AnswerResultViewModel> AnswerQuestionAsync(int sessionId, int answererId, AnswerViewModel answer);
        Task<ExamResultViewModel> GetResultForAsync(int resultId, int studentId);
    }
}