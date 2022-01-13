using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    internal static class Extentions
    {
        internal static async Task<StudentViewModel> InitStudent(this ControllerBase controller, IStudentService studentService)
        {
            string logId;
            if(!controller.TryGetLogIdFromCookie(studentService, out logId))return null;
            StudentViewModel student = await studentService.FindStudentAsync(logId);
            if(student == null)return null;
            if(controller is Controller)((Controller)controller).ViewBag.StudentFullName = student.FullName;
            return student;
        }

        internal static bool TryGetLogIdFromCookie(this ControllerBase controller, IStudentService studentService, out string logId){
            logId = controller.HttpContext.User.FindFirstValue("StudentLogId");
            if(logId == null)return false;
            return true;
        }
    } 
}