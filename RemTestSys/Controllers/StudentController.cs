using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Exams()
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return RedirectToAction("Login");
            Student student = await dbContext.Students.SingleOrDefaultAsync(s => s.LogId == logId);
            if (student == null) return RedirectToAction("Login");
            var exams = await dbContext.Exams.Where(ex => ex.AssignedTo.Id == student.Id)
                                             .Include(ex => ex.Test)
                                             .ToArrayAsync();
            var vmList = new List<ExamInfoViewModel>();
            foreach (var ex in exams)
            {
                var exInfo = new ExamInfoViewModel
                {
                    TestId = ex.Test.Id,
                    TestName = ex.Test.Name,
                    TestDescription = ex.Test.Description,
                    Status = ex.Status,
                    CountOfQuestions = ex.Test.QuestionsCount,
                    Duration = ex.Test.Duration,
                    Deadline = ex.Deadline
                };
                vmList.Add(exInfo);
            }
            return View(vmList);
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return RedirectToAction("Login");
            Student student = await dbContext.Students.SingleOrDefaultAsync(s => s.LogId == logId);
            if (student == null) return RedirectToAction("Login");
            AccessToTest accessToTest = await dbContext.AccessesToTest.FirstOrDefaultAsync(at => at.Student.Id == student.Id && at.Test.Id == id);
            if (accessToTest == null) return View("Error");
            Test test = await dbContext.Tests
                                       .Where(t => t.Id == id)
                                       .Include(t=>t.Questions)
                                       .SingleAsync();

            Session session = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Student.Id == student.Id && s.Test.Id == id);
            if (session != null && session.Finished)
            {
                dbContext.Sessions.Remove(session);
                await dbContext.SaveChangesAsync();
                session = null;
            }
            if(session == null)
            {
                session = sessionBuilder.Build(test, student);
                session.StartTime = DateTime.Now;
                dbContext.Sessions.Add(session);
                await dbContext.SaveChangesAsync();
            }
            return View(new TestingViewModel
            {
                SessionId = session.Id,
                TestName = test.Name,
                QuestionsCount = test.QuestionsCount
            });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid && login.StudentLogId.Length > 0)
            {
                Student student = await dbContext.Students.SingleOrDefaultAsync(s => s.LogId == login.StudentLogId);
                if (student != null)
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("StudentLogId", login.StudentLogId));
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    return RedirectToAction("Exams");
                }
                else
                {
                    ModelState.AddModelError("", "Учня з вказанним ідентифікатором не знайдено");
                }
            }
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Student");
        }
    }
}
