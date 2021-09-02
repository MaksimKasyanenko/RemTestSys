using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using RemTestSys.Domain;
using RemTestSys.ViewModel;
using RemTestSys.Extensions;

namespace RemTestSys.Controllers
{
    [ApiController]
    [Route("api/[controller]/{sessionId?}")]
    public class TestingController : ControllerBase
    {
        public TestingController(ISessionService testingService)
        {
            _testingService = testingService ?? throw new ArgumentNullException(nameof(ISessionService));
        }
        private readonly ISessionService _testingService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Answer(AnswerViewModel answer)
        {
            string logId;
            if (this.TryGetLogIdFromCookie(out logId))
            {
                AnswerResult result = await _testingService.Answer(logId, answer.SessionId, answer.Data);
                AnswerResultViewModel ar = new AnswerResultViewModel
                {
                    IsRight = result.IsRight,
                    RightText = result.RightText
                };
                return new ObjectResult(ar);
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetState(int sessionId){
            string logId;
            if (this.TryGetLogIdFromCookie(out logId))
            {
                Session session = await _testingService.FindSessionFor(logId, sessionId);
                TestingViewModel si;
                if (session != null)
                {
                    si = new TestingViewModel
                    {
                        SessionId = session.Id,
                        Finished = session.Finished,
                        QuestionNum = session.QuestionNum,
                        TimeLeft = session.TimeLeft,
                        QuestionText = session.CurrentQuestion.Text,
                        QuestionSubText = session.CurrentQuestion.SubText,
                        AnswerType = nameof(session.CurrentQuestion.Answer),
                        Addition = session.CurrentQuestion.Answer.Addition
                    };
                    return new ObjectResult(si);
                }
            }
            return BadRequest();
        }
    }
}
