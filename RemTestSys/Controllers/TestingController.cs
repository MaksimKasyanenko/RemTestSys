using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using RemTestSys.ViewModel;
using RemTestSys.Extensions;
using RemTestSys.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace RemTestSys.Controllers
{
    [ApiController]
    [Route("api/[controller]/{sessionId?}")]
    public class TestingController : ControllerBase
    {
        public TestingController(AppDbContext appDbContext)
        {
            this.dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext dbContext;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetState(int sessionId)
        {
            string logId;
            if (!this.TryGetLogIdFromCookie(out logId)) return BadRequest("Specified LogId is not valid");
            Session session = await dbContext.Sessions
                                                .Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                .Include(s=>s.Test)
                                                .Include(s=>s.Student)
                                                .Include(s=>s.Questions)
                                                .ThenInclude(q=>q.Question)
                                                .ThenInclude(q=>q.Answer)
                                                .SingleOrDefaultAsync();
            if(session != null)
            {
                TestingViewModel vm;
                if (session.Finished)
                {
                    vm = new TestingViewModel
                    {
                        SessionId = session.Id,
                        Finished = session.Finished,
                        QuestionNum = session.QuestionNum,
                        TimeLeft = session.TimeLeft,
                        ResultId = session.ResultId
                    };
                }
                else
                {
                    vm = new TestingViewModel
                    {
                        SessionId = session.Id,
                        TestName = session.Test.Name,
                        QuestionsCount = session.Test.QuestionsCount,
                        Finished = session.Finished,
                        QuestionNum = session.QuestionNum,
                        TimeLeft = session.TimeLeft,
                        QuestionText = session.CurrentQuestion.Text,
                        QuestionSubText = session.CurrentQuestion.SubText,
                        AnswerType = nameof(session.CurrentQuestion.Answer),
                        Addition = session.CurrentQuestion.Answer.Addition,
                        ResultId = session.ResultId
                    };
                }
                return new ObjectResult(vm);
            }
            else
            {

                return BadRequest($"Specified student haven't got an access to session({sessionId})");
            }
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Answer([FromRoute]int sessionId, [FromBody]AnswerViewModel answer)
        {
            string logId;
            if (this.TryGetLogIdFromCookie(out logId))
            {
                Session session = await dbContext.Sessions.Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                          .Include(s=>s.Questions)
                                                          .ThenInclude(qs=>qs.Question)
                                                          .ThenInclude(q=>q.Answer)
                                                          .Include(s=>s.Student)
                                                          .Include(s=>s.Test)
                                                          .SingleOrDefaultAsync();
                if (session != null)
                {
                    bool isRight = session.CurrentQuestion.Answer.IsMatch(answer.Data);
                    if (isRight)
                    {
                        session.RightAnswersCount++;
                    }
                    AnswerResultViewModel ar = new AnswerResultViewModel
                    {
                        IsRight = isRight,
                        RightText = isRight ? null : session.CurrentQuestion.Answer.RightText
                    };
                    if (!session.NextQuestion())
                    {
                        session.Finished = true;
                        ResultOfTesting resultOfTesting = new ResultOfTesting
                        {
                            Student = session.Student,
                            Test = session.Test,
                            Mark = session.GetMark()
                        };
                        dbContext.ResultsOfTesting.Add(resultOfTesting);
                        session.ResultId = resultOfTesting.Id;
                    }
                    await dbContext.SaveChangesAsync();
                    return new ObjectResult(ar);
                }
                else
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
