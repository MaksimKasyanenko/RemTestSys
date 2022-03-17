using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class ResultsController : Controller
    {
        public ResultsController(IExamService examService, IStudentService studentService, IGroupService groupService)
        {
            this.examService=examService??throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            this.groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        private readonly IExamService examService;
        private readonly IStudentService studentService;
        private readonly IGroupService groupService;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await examService.GetResultsForAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Student(int id)
        {
            var student = await studentService.FindStudentAsync(id);
            if(student == null)return NotFound();
            ViewData["StudentFullName"]=student.FullName;
            ViewData["StudentId"]=student.Id;
            return View(await examService.GetResultsForStudentAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Group(int id)
        {
            var group  = await groupService.FindAsync(id);
            if(group == null)return NotFound();
            ViewData["GroupName"] = group.Name;
            ViewData["GroupId"] = group.Id;
            return View(await examService.GetResultsForGroupAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Test(int id)
        {
            var exam = await examService.FindExamAsync(id);
            if(exam == null)return NotFound();
            ViewData["TestName"]=exam.Name;
            ViewData["TestId"]=exam.Id;
            return View(await examService.GetResultsOfExamAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> ClearAll()
        {
            await examService.RemoveAllResultsAsync();
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForGroup(int id)
        {
            await examService.RemoveResultsForGroupAsync(id);
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForStudent(int id)
        {
            await examService.RemoveResultsForStudentAsync(id);
            return View(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ClearForTest(int id)
        {
            await examService.RemoveResultsOfExamAsync(id);
            return View(nameof(Index));
        }
        
    }
}
