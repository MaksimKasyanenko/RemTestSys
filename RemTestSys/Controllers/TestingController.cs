using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using RemTestSys.Domain;
using RemTestSys.ViewModel;
using RemTestSys.Extensions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.Exceptions;

namespace RemTestSys.Controllers
{
    [ApiController]
    [Route("api/[controller]/{sessionId?}")]
    public class TestingController : ControllerBase
    {
        public TestingController(ISessionService sessionService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(ISessionService));
        }
        private readonly ISessionService _sessionService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetState(int sessionId)
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return BadRequest("Specified LogId is not valid");
            try
            {
                Session session = await _sessionService.GetSessionFor(sessionId, logId);
                TestingViewModel vm;
                vm = new TestingViewModel
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
                return new ObjectResult(vm);
            }
            catch (DataAccessException)
            {
                return BadRequest($"Specified student haven't got an access to session({sessionId})");
            }
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Answer(AnswerViewModel answer)
        {
            string logId;
            if (this.TryGetLogIdFromCookie(out logId))
            {
                AnswerResult result = await _sessionService.Answer(logId, answer.SessionId, answer.Data);
                AnswerResultViewModel ar = new AnswerResultViewModel
                {
                    IsRight = result.IsRight,
                    RightText = result.RightText
                };
                return new ObjectResult(ar);
            }
            else
            {
                return BadRequest("Specified LogId is not valid");
            }
        }
    }
}
