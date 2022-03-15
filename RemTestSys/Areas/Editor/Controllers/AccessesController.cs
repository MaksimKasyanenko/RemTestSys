using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Editor")]
    public class AccessesController : Controller
    {
        public AccessesController(IExamAccessService accessService, IGroupService groupService, IStudentService studentService)
        {
            this.accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));
            this.groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly IExamAccessService accessService;
        private readonly IGroupService groupService;
        private readonly IStudentService studentService;
        public async Task<IActionResult> Index()
        {
            return View(await accessService.GetAccessListAsync());
        }
        public async Task<IActionResult> OpenAccessToTestForAll(int id)
        {
            await accessService.OpenCommonAccessAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> OpenAccessToTestForGroup(int id)
        {
            ViewData["TestId"] = id;
            var groups = await groupService.GetGroupListAsync();
            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenAccessToTestForGroup(int id, int testId)
        {
            await accessService.OpenGroupAccessAsync(id, testId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> OpenAccessToTestForStudent(int id)
        {
            ViewData["TestId"] = id;
            var students = await studentService.GetStudentsAsync();
            return View(students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenAccessToTestForStudent(int id, int testId)
        {
            await accessService.OpenPersonAccessAsync(id, testId);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ClearAll()
        {
            await accessService.CloseAllAccessesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteForAll(int id)
        {
            await accessService.CloseCommonAccessAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForGroups(int id)
        {
            await accessService.CloseGroupAccessAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForStudent(int id)
        {
            await accessService.ClosePersonAccessAsync(id);
            return RedirectToAction("Index");
        }
    }
}
