using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IStudentService studentService, IGroupService groupService)
        {
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            this.groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        private readonly IStudentService studentService;
        private readonly IGroupService groupService;
        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            ViewData["Groups"] = await groupService.GetGroupListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(StudentViewModel studentData)
        {
            if (ModelState.IsValid)
            {
                string logId = await studentService.RegisterNewStudentAsync(studentData);
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim("StudentLogId", logId));
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                return RedirectToAction("AvailableTests", "Student");
            }
            else
            {
                ModelState.AddModelError("", "Необхідно вказати ім'я, прізвище, та групу(класс)");
            }
            ViewData["Groups"] = await groupService.GetGroupListAsync();
            return View(studentData);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Registration));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccountInfo()
        {
            var student = InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(student);
        }
    }
}
