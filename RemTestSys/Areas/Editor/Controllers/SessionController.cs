using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class SessionController : Controller
    {
        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }
        private readonly ISessionService sessionService;
        public async Task<IActionResult> Index()
        {
            return View(await sessionService.GetSessionListAsync());
        }
        public async Task<IActionResult> Close(int id)
        {
            await sessionService.CloseSessionAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CloseAll()
        {
            await sessionService.CloseAllSessionsAsync();
            return RedirectToAction("Index");
        }
    }
}
