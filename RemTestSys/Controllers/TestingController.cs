using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using RemTestSys.ViewModel;
using RemTestSys.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Controllers
{
    [ApiController]
    [Route("api/[controller]/{sessionId?}")]
    public class TestingController : ControllerBase
    {
        public TestingController(IStudentService studentService, IExamService examService, AppDbContext appDbContext)
        {
            this.dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            this.studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
        }
        private readonly AppDbContext dbContext;
        private readonly IExamService examService;
        private readonly IStudentService studentService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetState(int sessionId)
        {
            var student = await this.InitStudent(studentService);
            if (student == null) return BadRequest("Cookie doesn't contain nessesary data or one is invalid");
            ExamSessionViewModel state = await examService.GetSessionStateForAsync(sessionId, student.Id);
            if(state == null)
                return BadRequest($"Session doesn't exist or user doesn't have an access to it");
            return new ObjectResult(state);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Answer([FromRoute]int sessionId, [FromBody]AnswerViewModel answer)
        {
            string logId;
            if (this.TryGetLogIdFromCookie(studentService, out logId))
            {
                Session session = await dbContext.Sessions.Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                          .Include(s=>s.Questions)
                                                          .ThenInclude(qs=>qs.Question)
                                                          .ThenInclude(q=>q.Answer)
                                                          .Include(s=>s.Student)
                                                          .Include(s=>s.Test)
                                                          .ThenInclude(t=>t.MapParts)
                                                          .SingleOrDefaultAsync();
                if (session != null)
                {
                    bool isRight = session.CurrentQuestion.Answer.IsMatch(answer.Data);
                    if (isRight)
                    {
                        session.Scores+=session.CurrentQuestion.Cast;
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
                            Mark = session.GetMark(),
                            PassedAt = DateTime.Now
                        };
                        dbContext.ResultsOfTesting.Add(resultOfTesting);
                        await dbContext.SaveChangesAsync();
                        session.IdOfResult = resultOfTesting.Id;
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
