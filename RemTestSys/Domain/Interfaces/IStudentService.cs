using RemTestSys.Domain.ViewModels;

public interface IStudentService{
    Task RegisterNewStudentAsync(StudentRegistrationVM studentData);
    Task<StudentVM> FindStudentAsync(string logId);
}