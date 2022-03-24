using System;
using System.Linq;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;

namespace UnitTests.ServicesTests;

public class StudentServiceTests
{
    /***************************************************************************************************/
    public class RegisterNewStudentMethodTests:TestBase
    {
        public RegisterNewStudentMethodTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void CreatesNewStudentInDatabaseAndGeneratesLogId_WhenAllPropertiesAreProper()
        {
            using var context = CreateTransactionalContext();
            StudentService service = new StudentService(context);
            int groupId = context.Groups.First(g=>true).Id;
            StudentViewModel student = new StudentViewModel{
                FirstName = "newStudent",
                GroupId = groupId
            };

            string logId = await service.RegisterNewStudentAndGenerateLogIdAsync(student);
            context.ChangeTracker.Clear();

            Assert.Single(context.Students, s=>s.FirstName == "newStudent" && s.GroupId == groupId);
            Assert.Matches(@"\d{8}", logId);
            Assert.Equal(logId, context.Students.Single(s=>s.FirstName == "newStudent").LogId);
        }
        [Fact]
        public async void ThrowsInvalidOperationException_WhenSpecifiedGroupDoesNotExist()
        {
            using var context = CreateTransactionalContext();
            StudentService service = new StudentService(context);
            StudentViewModel student = new StudentViewModel{
                FirstName = "newStudent",
                GroupId = 1234
            };
            var action = async () => await service.RegisterNewStudentAndGenerateLogIdAsync(student);

            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
            Assert.Equal("The group with specified id doesn't exist", exception.Message);
        }
    }
    /***************************************************************************************************/



    /***************************************************************************************************/
}