using System;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace RemTestSys.Domain.Services{
    public class StudentService : IStudentService{

        public StudentService(AppDbContext dbContext){
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private readonly AppDbContext dbContext;

        public async Task<string> RegisterNewStudentAsync(StudentVM studentData){
            Student student = new Student {
                    FirstName = studentData.FirstName,
                    LastName = studentData.LastName,
                    RegistrationDate = DateTime.Now
                };
            Group group = await dbContext.Groups.FirstOrDefaultAsync(g=>g.Id == studentData.GroupId);
            student.Group = group ?? throw new InvalidOperationException($"The group with specified id({studentData.GroupId}) doesn't exist");
            student.LogId = await RandomLogId();
            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();
            return student.LogId;
        }

        public async Task<StudentVM> FindStudentAsync(string logId){
            return await dbContext.Students
                                  .Where(s => s.LogId == logId)
                                  .Include(s => s.Group)
                                  .Select(s => new StudentVM{
                                      FirstName = s.FirstName,
                                      LastName = s.LastName,
                                      GroupId = s.Group.Id,
                                      GroupName = s.Group.Name,
                                      RegistrationDate = s.RegistrationDate
                                  })
                                  .SingleOrDefaultAsync();
        }
        public async Task<bool> DoesStudentExistAsync(string logId)
        {
            return await dbContext.Students.AnyAsync(s => s.LogId == logId);
        }
        private async Task<string> RandomLogId()
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            int counter = 0;
            do
            {
                counter++;
                if (counter > 50) throw new InvalidOperationException("LogId cannot be generated");
                sb.Clear();
                for (int i = 0; i < 8; i++)
                {
                    sb.Append(rnd.Next(0, 10));
                }
            } while (await dbContext.Students.AnyAsync(s => s.LogId == sb.ToString()));
            return sb.ToString();
        }
    }
}