using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class ResultsController : Controller
    {
        public ResultsController(IExamResultService resultService, IStudentService studentService, IGroupService groupService, IExamService examService)
        {
            this.resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            this.groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
        }
        private readonly IExamResultService resultService;
        private readonly IStudentService studentService;
        private readonly IGroupService groupService;
        private readonly IExamService examService;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await resultService.GetResultsAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Student(int id)
        {
            var student = await studentService.FindStudentAsync(id);
            if(student == null)return NotFound();
            ViewData["StudentFullName"]=student.FullName;
            ViewData["StudentId"]=student.Id;
            return View(await resultService.GetResultsOfStudentAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Group(int id)
        {
            var group  = await groupService.FindAsync(id);
            if(group == null)return NotFound();
            ViewData["GroupName"] = group.Name;
            ViewData["GroupId"] = group.Id;
            return View(await resultService.GetResultsOfGroupAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Test(int id)
        {
            var exam = await examService.FindExamAsync(id);
            if(exam == null)return NotFound();
            ViewData["TestName"]=exam.Name;
            ViewData["TestId"]=exam.Id;
            return View(await resultService.GetResultsOfExamAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> ClearAll()
        {
            await resultService.ClearResultsAsync();
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForGroup(int id)
        {
            await resultService.ClearResultsOfGroupAsync(id);
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForStudent(int id)
        {
            await resultService.ClearResultsOfStudentAsync(id);
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForTest(int id)
        {
            await resultService.ClearResultsOfExamAsync(id);
            return View(nameof(Index));
        }
        
    }
}
