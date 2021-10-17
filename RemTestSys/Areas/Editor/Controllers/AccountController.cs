
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Areas.Editor.ViewModel;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                await Task.CompletedTask;
                return Redirect("www.google.com");
            }
            else
            {
                ModelState.AddModelError("","Потрібно указати і логін і пароль");
                return View(loginViewModel);
            }
        }
    }
}
