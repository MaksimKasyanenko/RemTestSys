using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RemTestSys.Extensions;
using RemTestSys.Domain;

namespace RemTestSys.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IStudentService studentService, IGroupService groupService)
        {
            this.studentService = studentService ?? throw new ArgumentNullReferenceException(nameof(studentService));
            thi.sgroupService = groupService ?? throw new ArgumentNullReferenceException(nameof(groupService));
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
        public async Task<IActionResult> Registration(StudentRegistrationVM studentData)
        {
            if (ModelState.IsValid)
            {
                Student student = await studentService.RegisterNewStudentAsync(studentData);
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim("StudentLogId", student.LogId));
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
            return View(regData);
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
            string logId;
            Student student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) throw new NullReferenceException(nameof(student));
            return View(student);
        }
    }
}
