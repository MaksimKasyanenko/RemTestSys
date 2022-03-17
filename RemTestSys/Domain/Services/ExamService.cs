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
        public ExamService(AppDbContext dbContext, ISessionBuilder sessionBuilder)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.sessionBuilder = sessionBuilder ?? throw new ArgumentNullException(nameof(sessionBuilder));
        }
        private readonly AppDbContext dbContext;
        private readonly ISessionBuilder sessionBuilder;
        public async Task<List<ExamViewModel>> GetExamsAsync()
        {
            return await dbContext.Tests
                            .Select(e => new ExamViewModel{
                                Id = e.Id,
                                Name = e.Name,
                                Description = e.Description,
                                QuestionCount = e.QuestionsCount,
                                Duration = e.Duration,
                                MaxMark = e.ScoreSum.ToString(),
                                MapParts = e.MapParts.Select(mp => new ExamViewModel.MapPart{
                                    QuestionCount = mp.QuestionCount,
                                    QuestionCost = mp.QuestionCast
                                })
                            }).ToListAsync();
        }
        public async Task<ExamViewModel> FindExamAsync(int id)
        {
            var exam = await dbContext.Tests.FirstAsync(e => e.Id == id);
            if(exam == null)return null;
            return new ExamViewModel{
                Id = exam.Id,
                Name = exam.Name,
                Description = exam.Description,
                QuestionCount = exam.QuestionsCount,
                Duration = exam.Duration,
                MaxMark = exam.ScoreSum.ToString(),
                MapParts = exam.MapParts.Select(mp => new ExamViewModel.MapPart{
                    QuestionCount = mp.QuestionCount,
                    QuestionCost = mp.QuestionCast
                })
            };
        }
        public async Task CreateExamAsync(ExamViewModel examViewModel)
        {
            Test exam = new Test{
                Name = examViewModel.Name,
                Description = examViewModel.Description,
                Duration = examViewModel.Duration,
                MapParts = examViewModel.MapParts.Select(mp => new Test.MapPart{
                    QuestionCount = mp.QuestionCount,
                    QuestionCast = mp.QuestionCost
                })
            };
            dbContext.Tests.Add(exam);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateExamAsync(ExamViewModel examViewModel)
        {
            var exam = await dbContext.Tests.FirstOrDefaultAsync(e => e.Id == examViewModel.Id);
            if(exam == null)throw new DbUpdateException("Attempt of updating unexisting exam");
            exam.Name = examViewModel.Name;
            exam.Description = examViewModel.Description;
            exam.Duration = examViewModel.Duration;
            exam.MapParts = examViewModel.MapParts.Select(mp => new Test.MapPart{
                QuestionCount = mp.QuestionCount,
                QuestionCast = mp.QuestionCost
            });
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteExamAsync(int id)
        {
            var exam = await dbContext.Tests.FirstOrDefaultAsync(e => e.Id == id);
            if(exam == null)throw new DbUpdateException("Attempt of deleting unexisting exam");
            dbContext.Tests.Remove(exam);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<ExamViewModel>> GetAvailableExamsForAsync(int studentId)
        {
            var tests = await dbContext.AccessesToTestForAll
                                       .Include(a => a.Test.MapParts)
                                       .Select(at => at.Test)
                                       .ToListAsync();
            int groupId = (await dbContext.Students.SingleAsync(s => s.Id == studentId)).GroupId;
            tests.AddRange(await dbContext.AccessesToTestForGroup
                                          .Where(a => a.GroupId == groupId)
                                          .Include(a => a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());
            tests.AddRange(await dbContext.AccessesToTestForStudent
                                          .Where(a => a.StudentId == studentId)
                                          .Include(a => a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());

            var vmList = new List<ExamViewModel>();
            foreach (var tst in tests)
            {
                string maxMark = "-";
                ResultOfTesting maxRes = await dbContext.ResultsOfTesting
                                                         .Where(r => r.Student.Id == studentId && r.Test.Id == tst.Id)
                                                         .OrderByDescending(r => r.Mark)
                                                         .FirstOrDefaultAsync();
                if (maxRes != null)
                {
                    maxMark = maxRes.Mark.ToString();
                }
                var tstInfo = new ExamViewModel
                {
                    Id = tst.Id,
                    Name = tst.Name,
                    Description = tst.Description,
                    QuestionCount = tst.QuestionsCount,
                    Duration = tst.Duration,
                    MaxMark = maxMark
                };
                vmList.Add(tstInfo);
            }
            return vmList;
        }

        public async Task<IEnumerable<ExamResultViewModel>> GetResultsForAsync(int studentId)
        {
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

        public async Task<bool> HasAccessToAsync(int studentId, int examId)
        {
            Student student = await dbContext.Students.SingleAsync(s => s.Id == studentId);
            return await dbContext.AccessesToTestForAll.AnyAsync(a => a.Test.Id == examId)
                || await dbContext.AccessesToTestForGroup.AnyAsync(a => a.Test.Id == examId && a.GroupId == student.GroupId)
                || await dbContext.AccessesToTestForStudent.AnyAsync(a => a.Test.Id == examId && a.StudentId == student.Id);
        }

        public async Task<ExamSessionViewModel> ExamineAsync(int studentId, int examId)
        {
            if (!await HasAccessToAsync(studentId, examId))
                throw new AccessToExamException($"Student {studentId} hasn't got access to the exam {examId}");
            Session session = await dbContext.Sessions
                                             .Include(s => s.Test)
                                             .ThenInclude(t => t.MapParts)
                                             .SingleOrDefaultAsync(s => s.Student.Id == studentId && s.Test.Id == examId);
            if (session != null && session.Finished)
            {
                dbContext.Sessions.Remove(session);
                await dbContext.SaveChangesAsync();
                session = null;
            }
            if (session == null)
            {
                Test test = await dbContext.Tests
                                       .Where(t => t.Id == examId)
                                       .Include(t => t.Questions)
                                       .Include(t => t.MapParts)
                                       .SingleAsync();
                Student student = await dbContext.Students
                                            .SingleAsync(s => s.Id == studentId);
                session = sessionBuilder.Build(test, student);
                session.StartTime = DateTime.Now;
                dbContext.Sessions.Add(session);
                await dbContext.SaveChangesAsync();
            }
            return new ExamSessionViewModel
            {
                SessionId = session.Id,
                QuestionsCount = session.Test.QuestionsCount,
                TestName = session.Test.Name
            };
        }

        public async Task<ExamResultViewModel> GetResultForAsync(int resultId, int studentId)
        {
            ResultOfTesting result = await dbContext.ResultsOfTesting
                                                    .Where(r => r.Id == resultId)
                                                    .Include(r => r.Test)
                                                    .SingleOrDefaultAsync();
            if (result != null)
            {
                if (result.StudentId != studentId)
                    throw new AccessToResultException($"Student {studentId} don't have access to result {resultId} as one isn't owner of it");
                ExamResultViewModel resVM = new ExamResultViewModel
                {
                    TestName = result.Test.Name,
                    Mark = result.Mark.ToString(),
                    PassedAt = result.PassedAt,
                    Id = result.Id
                };
                return resVM;
            }
            else
            {
                return null;
            }
        }

        public async Task<ExamSessionViewModel> GetSessionStateForAsync(int sessionId, int studentId)
        {
            Session session = await dbContext.Sessions
                                                .Where(s => s.Id == sessionId && s.Student.Id == studentId)
                                                .Include(s => s.Test)
                                                .ThenInclude(t => t.MapParts)
                                                .Include(s => s.Student)
                                                .Include(s => s.Questions)
                                                .ThenInclude(q => q.Question)
                                                .ThenInclude(q => q.Answer)
                                                .SingleOrDefaultAsync();
            ExamSessionViewModel state = null;
            if (session != null)
            {
                state = new ExamSessionViewModel
                {
                    SessionId = session.Id,
                    TestName = session.Test.Name,
                    Finished = session.Finished,
                    QuestionsCount = session.Test.QuestionsCount,
                    QuestionNum = session.QuestionNum,
                    TimeLeft = session.TimeLeft
                };
                if (session.Finished)
                {
                    state.ResultId = session.IdOfResult;
                }
                else
                {
                    state.QuestionText = session.CurrentQuestion.Text;
                    state.QuestionSubText = session.CurrentQuestion.SubText;
                    state.QuestionCost = session.CurrentQuestion.Cast;
                    state.AnswerType = session.CurrentQuestion.Answer.GetType().Name;
                    state.Addition = session.CurrentQuestion.Answer.GetAdditiveData();
                }
            }
            return state;
        }

        public async Task<AnswerResultViewModel> AnswerQuestionAsync(int sessionId, int answererId, AnswerViewModel answer)
        {
            Session session = await dbContext.Sessions.Where(s => s.Id == sessionId)
                                                          .Include(s=>s.Questions)
                                                          .ThenInclude(qs=>qs.Question)
                                                          .ThenInclude(q=>q.Answer)
                                                          .Include(s=>s.Student)
                                                          .Include(s=>s.Test)
                                                          .ThenInclude(t=>t.MapParts)
                                                          .SingleOrDefaultAsync();
            if (session == null)throw new NullReferenceException($"Session({sessionId}) not exists");
            if(session.Student.Id != answererId)throw new AccessToSessionException($"Specified student({answererId}) don't have access to session({session.Id})");
                
            bool isRight = session.CurrentQuestion.Answer.IsMatch(answer.Data);
            if (isRight)
            {
                session.Scores+=session.CurrentQuestion.Cast;
            }
            AnswerResultViewModel ar = new AnswerResultViewModel
            {
                IsRight = isRight,
                RightText = isRight ? null : session.CurrentQuestion.Answer.RightText
            };
            if (!session.NextQuestion())
            {
                session.Finished = true;
                ResultOfTesting resultOfTesting = new ResultOfTesting
                {
                    Student = session.Student,
                    Test = session.Test,
                    Mark = session.GetMark(),
                    PassedAt = DateTime.Now
                };
                dbContext.ResultsOfTesting.Add(resultOfTesting);
                await dbContext.SaveChangesAsync();
                session.IdOfResult = resultOfTesting.Id;
            }
            await dbContext.SaveChangesAsync();
            return ar;
        }
    }
}