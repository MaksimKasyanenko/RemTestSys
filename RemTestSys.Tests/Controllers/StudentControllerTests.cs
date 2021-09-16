using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
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

            Assert.Null(res.ViewName);
        }

        [Fact]
        public void ReturnsViewResultAndContainingEmptyLoginField_WhenPassedEmptyLoginField()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            var viewModel = new LoginViewModel { StudentLogId = "" };

            var res = (ViewResult)controller.Login(viewModel).Result;

            Assert.Equal("", ((LoginViewModel)res.Model).StudentLogId);
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
        public void ReturnsRedirectViewResultCalledExams_WhenPassedRightLogId()
        {
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.StudentExists(It.IsAny<string>()).Result)
                              .Returns(true);
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(aus => aus.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                           .Returns(Task.FromResult((object)null));
            var services = new ServiceCollection();
            services.AddSingleton(authServiceMock.Object);
            services.AddSingleton(new Mock<IUrlHelperFactory>().Object);
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = services.BuildServiceProvider()
                }
            };
            var viewModel = new LoginViewModel { StudentLogId = "RightLogId" };

            var res = (RedirectToActionResult)controller.Login(viewModel).Result;

            Assert.Equal("Exams", res.ActionName);
        }
    }

    public class StudentController_ExamsActionTests
    {
        [Fact]
        public void ReturnsRedirectToActionResultToLoginActionIfRequestDontAuthorized()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var res = (RedirectToActionResult)controller.Exams().Result;

            Assert.Equal("Login", res.ActionName);
        }
        [Fact]
        public void ReturnsViewResultWithNullNameAndContainingListWith2ExamsViewModel_IfRequestAuthorized()
        {
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.GetStudent(It.IsAny<string>()))
                              .Returns(Task.FromResult(new Student()));
            studentServiceMock.Setup(ss => ss.GetExamsForStudent(It.IsAny<int>()))
                              .Returns(Task.FromResult((IEnumerable<Exam>)new List<Exam> {
                                    new Exam(),
                                    new Exam()
                              }));
            var controller = new StudentController(studentServiceMock.Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim("StudentLogId", "TestStudent"));
            controller.HttpContext.User.AddIdentity(claimsIdentity);

            var res = (ViewResult)controller.Exams().Result;

            Assert.Equal(2, ((IEnumerable<ExamInfoViewModel>)res.Model).ToArray().Length);
            Assert.Null(res.ViewName);
        }
    }

    public class StudentController_TestingActionTests
    {
        [Fact]
        public void ReturnsRedirectToActionResultToLoginActionIfRequestDontAuthorized()
        {
            var controller = new StudentController(new Mock<IStudentService>().Object, new Mock<ISessionService>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var res = (RedirectToActionResult)controller.Testing(1).Result;

            Assert.Equal("Login", res.ActionName);
        }
        [Fact]
        public void ReturnsViewResultWithNullNameAndContainingSettedSessionId1TestNameABCQuestionCount5_IfRequestAuthorized()
        {
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(ss => ss.GetStudent(It.IsAny<string>()))
                              .Returns(Task.FromResult(new Student()));
            studentServiceMock.Setup(ss => ss.GetTestForStudent(It.IsAny<int>(), It.IsAny<int>()))
                              .Returns(Task.FromResult(
                                    new Test {Name="ABC",
                                    QuestionsCount=5}
                                  ));
            var sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(ss => ss.BeginOrContinue(It.IsAny<string>(), It.IsAny<int>()))
                              .Returns(Task.FromResult(
                                    new Session { Id=1}
                                  ));
            var controller = new StudentController(studentServiceMock.Object, sessionServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim("StudentLogId", "TestStudent"));
            controller.HttpContext.User.AddIdentity(claimsIdentity);

            var res = (ViewResult)controller.Testing(0).Result;

            Assert.Equal(1, ((TestingViewModel)res.Model).SessionId);
            Assert.Equal("ABC", ((TestingViewModel)res.Model).TestName);
            Assert.Equal(5, ((TestingViewModel)res.Model).QuestionsCount);
        }
    }
}
