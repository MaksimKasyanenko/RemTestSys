using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces;

public interface IExamResultService
{
    Task<IEnumerable<ExamResultViewModel>> GetResultsAsync();
    Task<ExamResultViewModel> FindResultAsync(int resultId);
    Task<IEnumerable<ExamResultViewModel>> GetResultsOfStudentAsync(int studentId);
    Task<IEnumerable<ExamResultViewModel>> GetResultsOfExamAsync(int examId);
    Task<IEnumerable<ExamResultViewModel>> GetResultsOfGroupAsync(int groupId);
    Task ClearResultsAsync();
    Task ClearResultsOfStudentAsync(int studentId);
    Task ClearResultsOfGroupAsync(int groupId);
    Task ClearResultsOfExamAsync(int examId);
}