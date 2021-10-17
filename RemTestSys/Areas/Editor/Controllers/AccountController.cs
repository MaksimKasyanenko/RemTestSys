
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Areas.Editor.ViewModel;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    public class AccountController : Controller
    {
        publuc AccountController(IConfiguration config)
        {
            Config = config??throw new ArgumentNullException(nameof(config));
        }
        private readonly IConfiguration Config;
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AntiforgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                if(loginViewModel.Login!=Config["EditorAccount:Login"]
                   || loginViewModel.Password!=Config["EditorAccount:Password"])
                {
                    ModelState.AddModelError("","Невірний логін або пароль");
                    return View(loginViewModel);
                }
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Editor"));
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Results");
            }
            else
            {
                ModelState.AddModelError("","Ïîòð³áíî óêàçàòè ³ ëîã³í ³ ïàðîëü");
                return View(loginViewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LogIn");
        }
    }
}
