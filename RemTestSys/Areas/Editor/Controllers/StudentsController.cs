using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain;
using RemTestSys.Domain.Interfaces;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class StudentsController : Controller
    {
        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly IStudentService studentService;
        public async Task<IActionResult> Index()
        {
            return View(await studentService.GetStudentsAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)return View("Error");
            var student = await studentService.FindStudentAsync((int)id);
            if (student == null)return NotFound();
            return View(student);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)return View("Error");
            var student = await studentService.FindStudentAsync((int)id);
            if (student == null)return NotFound();
            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await studentService.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
