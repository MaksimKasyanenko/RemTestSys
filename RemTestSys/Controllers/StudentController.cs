using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        public StudentController(IExamService examService, IStudentService studentService)
        {
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly IExamService examService;
        private readonly IStudentService studentService;

        public async Task<IActionResult> AvailableTests()
        {
            var student = this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await examService.GetAvailableExamsForAsync(student.Id));
        }

        public async Task<IActionResult> Results()
        {
            var student = this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await examService.GetResultsForAsync(student.Id));
        }

        public async Task<IActionResult> Testing(int id)
        {
            var student = this.InitStudent(studentService);
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
            var student = this.InitStudent(studentService);
            if (student == null) return RedirectToAction("Registration", "Account");
            ExamResultViewModel result;
            try{
                result = await examService.GetResultForAsync((int)id, student.Id);
            }catch(AccessToResultException ex){
                return View("Error");
            }
            if(result == null)
                return View("AvailableTests");
            return View(result);
        }
    }
}
