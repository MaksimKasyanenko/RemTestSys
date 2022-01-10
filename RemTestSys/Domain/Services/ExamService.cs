using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Services
{
    public class ExamService : IExamService
    {
        public ExamService(AppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext dbContext;

        public async Task<IEnumerable<ExamViewModel>> GetAvailableExamsForAsync(int studentId)
        {
            var tests = await dbContext.AccessesToTestForAll
                                       .Include(a=>a.Test.MapParts)
                                       .Select(at => at.Test)
                                       .ToListAsync();
            int groupId = (await dbContext.Students.SingleAsync(s => s.Id == studentId)).GroupId;
            tests.AddRange(await dbContext.AccessesToTestForGroup
                                          .Where(a => a.GroupId == groupId)
                                          .Include(a=>a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());
            tests.AddRange(await dbContext.AccessesToTestForStudent
                                          .Where(a => a.StudentId == studentId)
                                          .Include(a=>a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());

            var vmList = new List<ExamViewModel>();
            foreach (var tst in tests)
            {
                string lastMark = "-";
                ResultOfTesting lastRes = await dbContext.ResultsOfTesting
                                                         .Where(r => r.Student.Id == studentId && r.Test.Id == tst.Id)
                                                         .OrderByDescending(r => r.PassedAt)
                                                         .FirstOrDefaultAsync();
                if (lastRes != null)
                {
                    lastMark = lastRes.Mark.ToString();
                }
                var tstInfo = new ExamViewModel
                {
                    TestId = tst.Id,
                    TestName = tst.Name,
                    TestDescription = tst.Description,
                    CountOfQuestions = tst.QuestionsCount,
                    Duration = tst.Duration,
                    LastMark = lastMark
                };
                vmList.Add(tstInfo);
            }
            return vmList;
        }

        public async Task<IEnumerable<ExamResultViewModel>> GetResultsForAsync(int studentId){
            var results = await dbContext.ResultsOfTesting
                                         .Where(r => r.Student.Id == studentId)
                                         .OrderByDescending(r => r.PassedAt)
                                         .Take(20)
                                         .Include(r => r.Test)
                                         .ToArrayAsync();
            var resViewList = new List<ExamResultViewModel>(results.Length);
            foreach (var res in results)
            {
                resViewList.Add(
                        new ExamResultViewModel
                        {
                            TestName = res.Test.Name,
                            Mark = res.Mark.ToString(),
                            PassedAt = res.PassedAt
                        }
                    );
            }
            return resViewList;
        }

        public async Task<bool> HasAccessTo(int studentId, int examId){
            Student student = await dbContext.Students.SingleAsync(s => s.Id == studentId);
            return await dbContext.AccessesToTestForAll.AnyAsync(a => a.Test.Id == examId)
                || await dbContext.AccessesToTestForGroup.AnyAsync(a => a.Test.Id == examId && a.GroupId == student.GroupId)
                || await dbContext.AccessesToTestForStudent.AnyAsync(a => a.Test.Id == examId && a.StudentId == student.Id);
        }
    }
}