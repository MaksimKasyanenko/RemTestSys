using System;
using System.Linq;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.Models;

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
    public class FindStudentByLogIdTests:TestBase
    {
        public FindStudentByLogIdTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void RenurnsStudentWithSpecifiedLogId()
        {
            using var context = CreateContext();
            StudentService service = new StudentService(context);
            Student studentInDb = context.Students.First(s=>s.LogId=="87654321");

            StudentViewModel student = await service.FindStudentAsync("87654321");

            Assert.NotNull(student);
            Assert.Equal(studentInDb.Id, student.Id);
        }
        [Fact]
        public async void ReturnsNull_WhenStudentNotFound()
        {
            using var context = CreateContext();
            StudentService service = new StudentService(context);

            StudentViewModel student = await service.FindStudentAsync("222222222");

            Assert.Null(student);
        }
    }
}