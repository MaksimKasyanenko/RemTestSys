using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.ViewModel;
using RemTestSys.Extensions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RemTestSys.Controllers
{
    public class StudentController : Controller
    {
        public StudentController(AppDbContext appDbContext, ISessionBuilder sessionBuilder)
        {
            dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            this.sessionBuilder = sessionBuilder ?? throw new ArgumentNullException(nameof(sessionBuilder));
        }
        private readonly AppDbContext dbContext;
        private readonly ISessionBuilder sessionBuilder;

        [Authorize]
        public async Task<IActionResult> AvailableTests()
        {
            string logId;
            Student student=null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await dbContext.Students.Where(s => s.LogId == logId).Include(s=>s.Group).SingleOrDefaultAsync();
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            var tests = await dbContext.AccessesToTest
                                       .Where(at => at.EveryBody || at.Group.Id == student.Group.Id || at.Student.Id == student.Id)
                                       .Select(at => at.Test)
                                       .ToArrayAsync();
            var vmList = new List<TestInfoViewModel>();
            foreach (var tst in tests)
            {
                string lastMark = "-";
                ResultOfTesting lastRes = await dbContext.ResultsOfTesting
                                                         .Where(r => r.Student.Id == student.Id && r.Test.Id == tst.Id)
                                                         .OrderByDescending(r => r.PassedAt)
                                                         .FirstOrDefaultAsync();
                if(lastRes != null)
                {
                    lastMark = lastRes.Mark.ToString();
                }
                var tstInfo = new TestInfoViewModel
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
            return View(vmList);
        }

        [Authorize]
        public async Task<IActionResult> Results()
        {
            string logId;
            Student student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await dbContext.Students.Where(s => s.LogId == logId).SingleOrDefaultAsync();
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
            var results = await dbContext.ResultsOfTesting
                                         .Where(r => r.Student.Id == student.Id)
                                         .OrderByDescending(r=>r.PassedAt)
                                         .Take(10)
                                         .Include(r=>r.Test)
                                         .ToArrayAsync();
            List<ResultOfTestingViewModel> resViewList = new List<ResultOfTestingViewModel>(results.Length);
            foreach(var res in results)
            {
                resViewList.Add(
                        new ResultOfTestingViewModel {
                            TestName=res.Test.Name,
                            Mark = res.Mark.ToString(),
                            PassedAt= res.PassedAt
                        }
                    );
            }
            return View(resViewList);
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            string logId;
            Student student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await dbContext.Students.Where(s => s.LogId == logId).Include(s=>s.Group).SingleOrDefaultAsync();
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            AccessToTest accessToTest = await dbContext.AccessesToTest.FirstOrDefaultAsync(at => at.Test.Id == id && (at.EveryBody || at.Group.Id == student.Group.Id || at.Student.Id == student.Id));
            if (accessToTest == null) return View("Error");
            Session session = await dbContext.Sessions
                                             .Include(s=>s.Test)
                                             .ThenInclude(t=>t.Questions)
                                             .SingleOrDefaultAsync(s => s.Student.Id == student.Id && s.Test.Id == id);
            if (session != null && session.Finished)
            {
                dbContext.Sessions.Remove(session);
                await dbContext.SaveChangesAsync();
                session = null;
            }
            if(session == null)
            {
                Test test = await dbContext.Tests
                                       .Where(t => t.Id == id)
                                       .Include(t => t.Questions)
                                       .SingleAsync();
                session = sessionBuilder.Build(test, student);
                session.StartTime = DateTime.Now;
                dbContext.Sessions.Add(session);
                await dbContext.SaveChangesAsync();
            }
            return View(new TestingViewModel
            {
                SessionId = session.Id,
                QuestionsCount = session.Test.QuestionsCount,
                TestName = session.Test.Name
            });
        }

        [Authorize]
        public async Task<IActionResult> ResultOfTesting(int? id)
        {
            if (id == null) return return RedirectToAction("AvailableTests");
            string logId;
            Student student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await dbContext.Students.Where(s => s.LogId == logId).Include(s => s.Group).SingleOrDefaultAsync();
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            ResultOfTesting result = await dbContext.ResultsOfTesting
                                                    .Where(r => r.Id == id && r.Student.Id == student.Id)
                                                    .Include(r=>r.Test)
                                                    .SingleOrDefaultAsync();
            if (result != null)
            {
                ResultOfTestingViewModel vm = new ResultOfTestingViewModel {
                    TestName = result.Test.Name,
                    Mark = result.Mark.ToString()
                };
                return View(vm);
            }
            else
            {
                return RedirectToAction("AvailableTests");
            }
        }

        private void SetStudentNameToView(Student student)
        {
            ViewBag.StudentFullName = $"{student.FirstName} {student.LastName}";
        }
    }
}
