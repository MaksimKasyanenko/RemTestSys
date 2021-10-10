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
using System.Text;
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
        public async Task<IActionResult> Registration()
        {
            var groupList = await dbContext.Groups.Select(g => g.Name).ToArrayAsync();
            var regData = new RegistrationViewModel{
                GroupNameList=groupList
            };
            return View(regData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel regData)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student {
                    FirstName = regData.FirstName,
                    LastName = regData.LastName,
                    RegistrationDate = DateTime.Now
                };
                Group group = await dbContext.Groups.FirstOrDefaultAsync(g=>g.Name == regData.GroupName);
                student.Group = group;
                student.LogId = await RandomLogId();
                dbContext.Students.Add(student);
                await dbContext.SaveChangesAsync();

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
                ModelState.AddModelError("", "Необхідно вказати ім'я та прізвище");
            }
            regData.GroupNameList = await dbContext.Groups.Select(g => g.Name).ToArrayAsync();
            return View(regData);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Registration));
        }


        private async Task<string> RandomLogId()
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            int counter = 0;
            do
            {
                counter++;
                if (counter > 50) throw new InvalidOperationException("LogId cannot be generated");
                sb.Clear();
                for (int i = 0; i < 8; i++)
                {
                    sb.Append(rnd.Next(0, 10));
                }
            } while (await dbContext.Students.AnyAsync(s => s.LogId == sb.ToString()));
            return sb.ToString();
        }
    }
}
