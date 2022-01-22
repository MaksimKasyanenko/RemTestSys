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
            var student = await this.InitStudent(studentService);
            if (student == null) return BadRequest("Cookie doesn't contain nessesary data or one is invalid");
            return new ObjectResult(await examService.AnswerQuestionAsync(sessionId, answer, student));
        }
    }
}
