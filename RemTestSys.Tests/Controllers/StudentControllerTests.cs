using Microsoft.AspNetCore.Mvc;
using Moq;
using RemTestSys.Controllers;
using RemTestSys.Domain.Exceptions;
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
        public void WithoutParametersReturnsViewResultWithViewNameEqualsNull()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);

            var res = (ViewResult)controller.Login();

            Assert.True(res.ViewName == null);
        }

        [Fact]
        public void ReturnsViewResultAndContainingEmptyLoginField_WhenPassedEmptyLoginField()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            var viewModel = new LoginViewModel { StudentLogId = "" };

            var res = (ViewResult)controller.Login(viewModel).Result;

            Assert.True(((LoginViewModel)res.Model).StudentLogId=="");
        }

        [Fact]
        public void ReturnViewContainingWrongLogIdInLoginFieldAnd1ModelError_WhenPassedWrongLogId()
        {
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.GetStudent(It.IsAny<string>()))
                              .Throws(new NotExistException(""));
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            var viewModel = new LoginViewModel { StudentLogId = "logId" };

            var res = (ViewResult)controller.Login(viewModel).Result;

            Assert.Equal(viewModel.StudentLogId, ((LoginViewModel)res.Model).StudentLogId);
            Assert.True(controller.ModelState.Count == 1);
        }
        
        [Fact]
        public void ReturnRedirectToActionCalledExams(){
        	string rightLogId = "RightLogId";
        	var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.GetStudent(It.Is<string>(li => li == "RightLogId")))
                              .Return(new Student());
            Type expectedType = typeof(RedirectToAction);
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            var viewModel = new LoginViewModel { StudentLogId = rightLogId };

            var res = (ViewResult)controller.Login(viewModel).Result;
            
            Assert.Equal(expectedType, res);
            Assert.True(res.ViewName == "Exams");
        }
    }
    
    public class StudentController_ExamsActionTests{
    	[Fact]
    	public void ReturnsRedirectToActionResultToLoginActionIfRequestDontAuthorized(){
    		var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
    		
    		var res = (RedirectToActionResult)controller.Exams().Result;
    		
    		Assert.True(res.ViewName == "Login");
    	}
    }
}
