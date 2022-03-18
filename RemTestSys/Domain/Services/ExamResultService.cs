using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Services;
public class ExamResultService : IExamResultService
{
    public ExamResultService(AppDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    private readonly AppDbContext dbContext;
    public async Task<ExamResultViewModel> FindResultAsync(int resultId) => (await GetFilteredResultsAsync(r => r.Id == resultId)).FirstOrDefault();
    public async Task<IEnumerable<ExamResultViewModel>> GetResultsAsync() => await GetFilteredResultsAsync(r => true);
    public async Task<IEnumerable<ExamResultViewModel>> GetResultsOfStudentAsync(int studentId) => await GetFilteredResultsAsync(r => r.Student.Id == studentId);
    public async Task<IEnumerable<ExamResultViewModel>> GetResultsOfGroupAsync(int groupId) => await GetFilteredResultsAsync(r => r.Student.Group.Id == groupId);
    public async Task<IEnumerable<ExamResultViewModel>> GetResultsOfExamAsync(int examId) => await GetFilteredResultsAsync(r => r.TestId == examId);
    private async Task<IEnumerable<ExamResultViewModel>> GetFilteredResultsAsync(Expression<Func<ResultOfTesting, bool>> filter)
    {
        return await dbContext.ResultsOfTesting.Where(filter)
                                               .Select(r => new ExamResultViewModel{
            Id = r.Id,
            StudentId = r.StudentId,
            StudentName = r.Student.FullName,
            StudentGroupId = r.Student.GroupId,
            StudentGroupName = r.Student.Group.Name,
            TestId = r.TestId,
            TestName = r.Test.Name,
            Mark = r.Mark.ToString(),
            PassedAt = r.PassedAt
        }).ToListAsync();
    }
    public async Task ClearResultsAsync() => await ClearFilteredResultsAsync(r => true);
    public async Task ClearResultsOfStudentAsync(int studentId) => await ClearFilteredResultsAsync(r => r.StudentId == studentId);
    public async Task ClearResultsOfGroupAsync(int groupId) => await ClearFilteredResultsAsync(r => r.Student.GroupId == groupId);
    public async Task ClearResultsOfExamAsync(int examId) => await ClearFilteredResultsAsync(r => r.TestId == examId);
    private async Task ClearFilteredResultsAsync(Expression<Func<ResultOfTesting, bool>> filter)
    {
        dbContext.ResultsOfTesting.RemoveRange(dbContext.ResultsOfTesting.Where(filter));
        await dbContext.SaveChangesAsync();
    }
}