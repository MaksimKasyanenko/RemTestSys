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
    public class StudentController : Controller
    {
        public StudentController(IExamService examService, IStudentService studentService)
        {
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly IExamService examService;
        private readonly IStudentService studentService;

        [Authorize]
        public async Task<IActionResult> AvailableTests()
        {
            var student = InitStudent();
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await examService.GetAvailableExamsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Results()
        {
            var student = InitStudent();
            if (student == null) return RedirectToAction("Registration", "Account");
            return View(await examService.GetResultsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            var student = InitStudent();
            if (student == null) return RedirectToAction("Registration", "Account");
            ExamSessionViewModel session;
            try{
                session = await examService.ExamineAsync(student.Id, id);
            }catch(AccessToExamException ex){
                return View("Error");
            }
            return View(session);
        }

        [Authorize]
        public async Task<IActionResult> ResultOfTesting(int? id)
        {
            if (id == null) return RedirectToAction("AvailableTests");
            var student = InitStudent();
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

        private async Task<StudentViewModel> InitStudent(){
            string logId = this.HttpContext.User.FindFirstValue("StudentLogId");
            if(logId == null)return null;
            StudentViewModel student = await studentService.FindStudentAsync(logId);
            if(student == null)return null;
            ViewBag.StudentFullName = student.FullName;
            return student;
        }
    }
}
