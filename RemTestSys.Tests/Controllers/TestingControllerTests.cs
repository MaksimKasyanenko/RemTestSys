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
                    Id = 10,
                    QuestionNum = 1,
                    StartTime = new DateTime(2021,12,1,0,0,0),
                    Student=new Student(),
                    Test = new Test(),
                    Questions = new List<QuestionInSession> {
                        new QuestionInSession{
                            SerialNumber = 1,
                            Question = new Question{Answer = new Answer()}
                        }
                    }
                };
                var sessionServiceMock = new Mock<ISessionService>();
                sessionServiceMock.Setup(ss => ss.GetSessionFor(It.IsAny<int>(), It.IsAny<string>()))
                                  .Returns(Task.FromResult(testSession));
                var controller = CreateTestingControllerWithDefaultContext(sessionServiceMock.Object);
                AddClaimsIdentityToController(controller);

                var res = (ObjectResult)controller.GetState(1).Result;

                Assert.Equal(10, ((TestingViewModel)res.Value).SessionId);
                Assert.Equal(1, ((TestingViewModel)res.Value).QuestionNum);
            }
        }

        public class AnswerActionTests
        {
            [Fact]
            public void ReturnsBadRequestObjectResultAndStatusCode400_IfNotAuthorized()
            {
                var controller = CreateTestingControllerWithDefaultContext();

                var res = (BadRequestObjectResult)controller.Answer(new AnswerViewModel()).Result;

                Assert.Equal(400, res.StatusCode);
            }
            [Fact]
            public void ReturnsBadRequestObjectResultAndStatusCode400_IfSessionDoesntExistsForStudentWithSpecifiedLogId()
            {
                var sessionServiceMock = new Mock<ISessionService>();
                sessionServiceMock.Setup(ss => ss.Answer(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<object>()))
                                  .Throws(new NotExistException(""));
                var controller = CreateTestingControllerWithDefaultContext(sessionServiceMock.Object);
                AddClaimsIdentityToController(controller);

                var res = (BadRequestObjectResult)controller.Answer(new AnswerViewModel()).Result;

                Assert.Equal(400, res.StatusCode);
            }
            [Fact]
            public void ReturnsObjectResultWithAnswerResultViewModel()
            {
                var sessionServiceMock = new Mock<ISessionService>();
                sessionServiceMock.Setup(ss => ss.Answer(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<object>()))
                                  .Returns(Task.FromResult(new Domain.AnswerResult {RightText=null, IsRight=true}));
                var controller = CreateTestingControllerWithDefaultContext(sessionServiceMock.Object);
                AddClaimsIdentityToController(controller);

                var res = (ObjectResult)controller.Answer(new AnswerViewModel()).Result;

                Assert.True(((AnswerResultViewModel)res.Value).IsRight);
                Assert.Null(((AnswerResultViewModel)res.Value).RightText);
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
