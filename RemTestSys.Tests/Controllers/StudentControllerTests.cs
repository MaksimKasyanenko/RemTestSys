using Microsoft.AspNetCore.Mvc;
using Moq;
using RemTestSys.Controllers;
using RemTestSys.Domain.Interfaces;
using RemTestSys.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RemTestSys.Tests.Controllers
{
    public class StudentController_LoginActionTests
    {
        [Fact]
        public void WithoutParametersReturnsViewResult()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            Type expected = typeof(ViewResult);

            var res = controller.Login();

            Assert.IsType(expected, res);
        }

        [Fact]
        public void ReturnsViewResultContainingEmptyLoginField_WhenPassedEmptyLoginField()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            Type expectedType = typeof(ViewResult);
            var viewModel = new LoginViewModel { StudentLogId = "" };

            var res = (ViewResult)(controller.Login(viewModel).Result);

            Assert.IsType(expectedType, res);
            Assert.Equal("", ((LoginViewModel)res.Model).StudentLogId);
        }

        [Fact]
        public void ReturnViewResultContainingWrongLogIdInLoginFieldAnd1ModelError_WhenPassedWrongLogId()
        {
            throw new NotImplementedException();
        }
    }
}
