using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RemTestSys.Domain;
using RemTestSys.Extensions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    public class StudentController : Controller
    {
        public StudentController(IExamService examService, IStudentService studentService, AppDbContext appDbContext)
        {
            dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }
        private readonly AppDbContext dbContext;
        private readonly IExamService examService;
        private readonly IStudentService studentService;

        [Authorize]
        public async Task<IActionResult> AvailableTests()
        {
            string logId;
            StudentViewModel student = null;
            if (this.TryGetLogIdFromCookie(out logId))
                student = await studentService.FindStudentAsync(logId);
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
                student = await studentService.Find(logId);
            if (student == null) return RedirectToAction("Registration", "Account");
            SetStudentNameToView(student);

            /*ResultOfTesting result = await dbContext.ResultsOfTesting
                                                    .Where(r => r.Id == id && r.Student.Id == student.Id)
                                                    .Include(r => r.Test)
                                                    .SingleOrDefaultAsync();
            if (result != null)
            {
                ResultOfTestingViewModel vm = new ResultOfTestingViewModel
                {
                    TestName = result.Test.Name,
                    Mark = result.Mark.ToString()
                };
                return View(vm);
            }
            else
            {
                return RedirectToAction("AvailableTests");
            }*/
            ExamResultViewModel result;
            try{
                result = examService.GetResultAsync(id, student.Id);
                if(result == null)return View("AvailableTests");
                return View(result);
            }catch(AccessToResultException ex){
                return View("Error");
            }
        }

        private void SetStudentNameToView(StudentViewModel student)
        {
            ViewBag.StudentFullName = $"{student.FirstName} {student.LastName}";
        }
    }
}
