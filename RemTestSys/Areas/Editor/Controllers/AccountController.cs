
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
                !!!
            }
            else
            {
                ModelState.AddModelError("","Ïîòð³áíî óêàçàòè ³ ëîã³í ³ ïàðîëü");
                return View(loginViewModel);
            }
        }
    }
}
