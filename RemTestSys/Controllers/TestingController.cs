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
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RemTestSys.Controllers
{
    [ApiController]
    [Route("api/[controller]/{sessionId?}")]
    public class TestingController : ControllerBase
    {
        public TestingController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext appDbContext;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetState(int sessionId)
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return BadRequest("Specified LogId is not valid");
            Session session = await appDbContext.Sessions
                                                .Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                .Include(s=>s.Questions)
                                                .Include(s=>s.Questions.Select(q=>q.Question))
                                                .Include(s => s.Questions.Select(q => q.Question.Answer))
                                                .SingleOrDefaultAsync();
            if(session != null)
            {
                
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
            else
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
                try
                {
                    AnswerResult result = await _sessionService.Answer(logId, answer.SessionId, answer.Data);
                    AnswerResultViewModel ar = new AnswerResultViewModel
                    {
                        IsRight = result.IsRight,
                        RightText = result.RightText
                    };
                    return new ObjectResult(ar);
                }
                catch (NotExistException)
                {
                    return BadRequest("LogId or SessionId is wrong");
                }
            }
            else
            {
                return BadRequest("Specified LogId is not valid");
            }
        }
    }
}
