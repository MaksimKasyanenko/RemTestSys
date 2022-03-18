using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using RemTestSys.Domain;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        public StudentController(IExamService examService, IStudentService studentService, IExamResultService resultService)
        {
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            this.resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        }
        private readonly IExamService examService;
        private readonly IStudentService studentService;
        private readonly IExamResultService resultService;

        public async Task<IActionResult> AvailableTests()
        {
            StudentViewModel student = await this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await examService.GetAvailableExamsForAsync(student.Id));
        }

        public async Task<IActionResult> Results()
        {
            StudentViewModel student = await this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await resultService.GetResultsOfStudentAsync(student.Id));
        }

        public async Task<IActionResult> Testing(int id)
        {
            StudentViewModel student = await this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            ExamSessionViewModel session;
            try{
                session = await examService.ExamineAsync(student.Id, id);
            }catch(AccessToExamException ex){
                return View("Error");
            }
            return View(session);
        }

        public async Task<IActionResult> ResultOfTesting(int? id)
        {
            if (id == null) return RedirectToAction("AvailableTests");
            StudentViewModel student = await this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            var result = await resultService.FindResultAsync((int)id);
            if(result == null || result.StudentId != student.Id)
                return View("AvailableTests");
            return View(result);
        }
    }
}
