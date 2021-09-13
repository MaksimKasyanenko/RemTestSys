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
using RemTestSys.Domain.Exceptions;

namespace RemTestSys.Controllers
{
    public class StudentController : Controller
    {
        public StudentController(IStudentService studentService, ISessionService sessionService)
        {
            this._studentService = studentService ?? throw new ArgumentNullException(nameof(IStudentService));
            this._sessionService = sessionService ?? throw new ArgumentNullException(nameof(ISessionService));
        }
        private readonly IStudentService _studentService;
        private readonly ISessionService _sessionService;

        [Authorize]
        public async Task<IActionResult> Exams()
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return RedirectToAction("Login");
            Student student = await _studentService.GetStudent(logId);
            var exams = await _studentService.GetExamsForStudent(student.Id);
            var vmList=new List<ExamInfoViewModel>();
            foreach(var ex in exams)
            {
                vmList.Add(new ExamInfoViewModel(ex));
            }
            return View(vmList);
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return RedirectToAction("Login");
            Student student = await _studentService.GetStudent(logId);
            Test test = await _studentService.GetTestForStudent(id, student.Id);

            Session session = await _sessionService.BeginOrContinue(logId, test.Id);

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
            if (ModelState.IsValid && login.StudentLogId.Length>0)
            {
                Student student;
                try {
                    student = await _studentService.GetStudent(login.StudentLogId);
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("StudentLogId", student.LogId));
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    return RedirectToAction("Exams");
                }
                catch (NotExistException)
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
