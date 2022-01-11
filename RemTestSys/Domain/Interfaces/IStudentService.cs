using RemTestSys.Domain.ViewModels;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces{
    public interface IStudentService
    {
        Task<string> RegisterNewStudentAsync(StudentViewModel studentData);
        Task<StudentViewModel> FindStudentAsync(string logId);
        Task<bool> DoesStudentExistAsync(string logId);
    }
}