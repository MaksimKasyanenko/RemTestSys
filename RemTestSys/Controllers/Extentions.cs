using System.Threading.Tasks;
using System.Security.Claims;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    internal static class Extentions
    {
        internal static async Task<StudentViewModel> InitStudent(this Controller controller, IStudentService studentService)
        {
            string logId = this.HttpContext.User.FindFirstValue("StudentLogId");
            if(logId == null)return null;
            StudentViewModel student = await studentService.FindStudentAsync(logId);
            if(student == null)return null;
            ViewBag.StudentFullName = student.FullName;
            return student;
        }
    } 
}