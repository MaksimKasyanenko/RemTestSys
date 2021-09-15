using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RemTestSys.Controllers;
using RemTestSys.Domain.Exceptions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            studentServiceMock.Setup(ss => ss.StudentExists(It.IsAny<string>()).Result)
                              .Returns(false);
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            var viewModel = new LoginViewModel { StudentLogId = "logId" };

            var res = (ViewResult)controller.Login(viewModel).Result;

            Assert.Equal(viewModel.StudentLogId, ((LoginViewModel)res.Model).StudentLogId);
            Assert.True(controller.ModelState.Count == 1);
        }

        [Fact]
        public void WritesLogIdToHttpCookieAndReturnsRedirectViewResultCalledExams_WhenPassedRightLogId()
        {
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.StudentExists(It.IsAny<string>()).Result)
                              .Returns(true);
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(aus => aus.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);
            var servProvMock = new Mock<IServiceProvider>();
            servProvMock.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(new Mock<IAuthenticationService>().Object);
            controller.ControllerContext.HttpContext.RequestServices = servProvMock.Object;
            var viewModel = new LoginViewModel { StudentLogId = "RightLogId" };
            var expectedType = typeof(RedirectToActionResult);

            var res = (RedirectToActionResult)controller.Login(viewModel).Result;

            Assert.True(controller.HttpContext.User.Identity.IsAuthenticated);
            Assert.True(viewModel.StudentLogId == controller.User.FindFirstValue("StudentLogId"));
            Assert.True(res.ActionName == "Exams");
        }
    }
    
    public class StudentController_ExamsActionTests{
    	[Fact]
    	public void ReturnsRedirectToActionResultToLoginActionIfRequestDontAuthorized(){
    		var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
    		
    		var res = (RedirectToActionResult)controller.Exams().Result;
    		
    		Assert.True(res.ActionName == "Login");
    	}
    }
}
