using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RemTestSys.Domain;
using RemTestSys.ViewModel;
using RemTestSys.Extensions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    public class StudentController : Controller
    {
        public StudentController(IExamService examService, IStudentService studentService, AppDbContext appDbContext, ISessionBuilder sessionBuilder)
        {
            dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            this.sessionBuilder = sessionBuilder ?? throw new ArgumentNullException(nameof(sessionBuilder));
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly AppDbContext dbContext;
        private readonly ISessionBuilder sessionBuilder;
        private readonly IExamService examService;
        private readonly IStudentService studentService;

        [Authorize]
        public async Task<IActionResult> AvailableTests()
        {
            string logId;
            StudentVM student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
            return View(await examService.GetAvailableExamsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Results()
        {
            string logId;
            StudentVM student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
            return View(await examService.GetResultsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            string logId;
            StudentVM student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            if (!await examService.HasAccessTo(student.Id, id))
            {
                return View("Error");
            }
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Session session = await dbContext.Sessions
                                             .Include(s => s.Test)
                                             .ThenInclude(t=>t.MapParts)
                                             .SingleOrDefaultAsync(s => s.Student.Id == student.Id && s.Test.Id == id);
            if (session != null && session.Finished)
            {
                dbContext.Sessions.Remove(session);
                await dbContext.SaveChangesAsync();
                session = null;
            }
            if (session == null)
            {
                Test test = await dbContext.Tests
                                       .Where(t => t.Id == id)
                                       .Include(t => t.Questions)
                                       .Include(t=>t.MapParts)
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
            if (id == null) return RedirectToAction("AvailableTests");
            string logId;
            Student student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await dbContext.Students.Where(s => s.LogId == logId).Include(s => s.Group).SingleOrDefaultAsync();
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            ResultOfTesting result = await dbContext.ResultsOfTesting
                                                    .Where(r => r.Id == id && r.Student.Id == student.Id)
                                                    .Include(r => r.Test)
                                                    .SingleOrDefaultAsync();
            if (result != null)
            {
                ResultOfTestingViewModel vm = new ResultOfTestingViewModel
                {
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

        private void SetStudentNameToView(StudentVM student)
        {
            ViewBag.StudentFullName = $"{student.FirstName} {student.LastName}";
        }
    }
}
