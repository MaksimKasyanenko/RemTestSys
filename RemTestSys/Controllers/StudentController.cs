using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using RemTestSys.Domain;
using RemTestSys.Extensions;
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
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
            return View(await examService.GetAvailableExamsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Results()
        {
            string logId;
            StudentViewModel student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
            return View(await examService.GetResultsForAsync(student.Id));
        }

        [Authorize]
        public async Task<IActionResult> Testing(int id)
        {
            string logId;
            StudentViewModel student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
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
            string logId;
            StudentViewModel student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);
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

        private void SetStudentNameToView(StudentViewModel student)
        {
            
        }
        private async Task<bool> InitStudent(){
            string logId = this.HttpContext.User.FindFirstValue("StudentLogId");
            if(logId == null)return false;
            StudentViewModel student = await studentService.FindStudentAsync(logId);
            if(student == null)throw new WrongLogIdInCoockieException($"LogId which specified in cookie is wrong! Perhaps the student has been deleted.");
            ViewBag.StudentFullName = student.FullName;
            return true;
        }
    }
}
