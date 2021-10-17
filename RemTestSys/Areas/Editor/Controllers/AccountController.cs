
namespace RemTestSys.Areas.Editor.Controllers
{
[Area("Editor")]
public class AccountController : Controller
{
    [HttpGet]
    public async Task<IActionResult> LogIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel loginData)
    {

    }
}
}
