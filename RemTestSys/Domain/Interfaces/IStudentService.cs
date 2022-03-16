using RemTestSys.Domain.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RemTestSys.Domain.Interfaces{
    public interface IStudentService
    {
        Task<string> RegisterNewStudentAsync(StudentViewModel studentData);
        Task<StudentViewModel> FindStudentAsync(string logId);
        Task<StudentViewModel> FindStudentAsync(int id);
        Task<bool> DoesStudentExistAsync(string logId);
        Task<List<StudentViewModel>> GetStudentsAsync();
        Task DeleteStudentAsync(int id);
    }
}