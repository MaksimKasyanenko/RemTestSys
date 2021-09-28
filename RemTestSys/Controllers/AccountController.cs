using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using RemTestSys.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RemTestSys.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(AppDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            this.dbContext = dbContext;
        }
        private readonly AppDbContext dbContext;
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
                    return RedirectToAction("Exams", "Student");
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
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            StudentViewModel st = new StudentViewModel();
            st.GroupList = await dbContext.Groups.Select(g => g.Name).ToArrayAsync();
            return View(st);
        }
        [HttpPost]
        public async Task<IActionResult> Registration(StudentViewModel newStudent)
        {
            StudentViewModel st = new StudentViewModel();
            st.GroupList = await dbContext.Groups.Select(g => g.Name).ToArrayAsync();
            return View(st);
        }
    }
}
