using RemTestSys.Domain.ViewModels;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces{
    public interface IStudentService
    {
        Task<string> RegisterNewStudentAsync(StudentVM studentData);
        Task<StudentVM> FindStudentAsync(string logId);
        Task<bool> DoesStudentExistAsync(string logId);
    }
}