using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using RemTestSys.Controllers;
using RemTestSys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RemTestSys.Domain.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using RemTestSys.Domain.Models;
using RemTestSys.ViewModel;

namespace RemTestSys.Tests.Controllers
{
    public class TestingControllerTests
    {
        public class GetStateActionTests
        {
            [Fact]
            public void ReturnsBadRequestObjectResultAndStatusCode400_IfNotAuthorized()
            {
                var controller = CreateTestingControllerWithDefaultContext();

                var res = (BadRequestObjectResult)controller.GetState(1).Result;

                Assert.Equal(400, res.StatusCode);
            }
            [Fact]
            public void ReturnsBadRequestObjectResultAndStatusCode400_IfSessionDoesntExistsForStudentWithSpecifiedLogId()
            {
                var sessionServiceMock = new Mock<ISessionService>();
                sessionServiceMock.Setup(ss => ss.GetSessionFor(It.IsAny<int>(), It.IsAny<string>()))
                                  .Throws(new DataAccessException(""));
                var controller = CreateTestingControllerWithDefaultContext(sessionServiceMock.Object);
                AddClaimsIdentityToController(controller);

                var res = (BadRequestObjectResult)controller.GetState(1).Result;

                Assert.Equal(400, res.StatusCode);
            }
            [Fact]
            public void ReturnsObjectResultWithSomeFilledFields_WhenSessionIsFoundForStudent()
            {
                var testSession = new Session {
                    Id = 1,
                    QuestionNum = 10,
                    StartTime = new DateTime(2021,12,1,0,0,0)
                };
                var sessionServiceMock = new Mock<ISessionService>();
                sessionServiceMock.Setup(ss => ss.GetSessionFor(It.IsAny<int>(), It.IsAny<string>()))
                                  .Returns(Task.FromResult(testSession));
                var controller = CreateTestingControllerWithDefaultContext(sessionServiceMock.Object);
                AddClaimsIdentityToController(controller);

                var res = (ObjectResult)controller.GetState(1).Result;

                Assert.Equal(1, ((TestingViewModel)res.Value).SessionId);
                Assert.Equal(10, ((TestingViewModel)res.Value).QuestionNum);
            }
        }








        //Utils
        private static TestingController CreateTestingControllerWithDefaultContext(ISessionService sessionServiceMock)
        {
            var res =  new TestingController(sessionServiceMock);
            res.ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            };
            return res;
        }
        private static TestingController CreateTestingControllerWithDefaultContext()
        {
            return CreateTestingControllerWithDefaultContext(new Mock<ISessionService>().Object);
        }
        private static void AddClaimsIdentityToController(TestingController controller)
        {
            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim("StudentLogId", "TestStudent"));
            controller.HttpContext.User.AddIdentity(claimsIdentity);
        }
    }
}
