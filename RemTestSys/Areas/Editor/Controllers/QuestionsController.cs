using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels.QuestionsWithAnswers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Editor")]
    public class QuestionsController : Controller
    {
        public QuestionsController(IQuestionService questionService, IExamService examService, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
        }
        private readonly IExamService examService;
        private readonly IQuestionService questionService;
        private readonly AppDbContext dbContext;


        [HttpGet]
        public async Task<IActionResult> CreateTextAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateTextAnswer), id);
        [HttpGet]
        public async Task<IActionResult> CreateOneOfFourAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateOneOfFourAnswer), id);
        [HttpGet]
        public async Task<IActionResult> CreateSomeAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateSomeAnswer), id);
        [HttpGet]
        public async Task<IActionResult> CreateSequenceAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateSequenceAnswer), id);
        [HttpGet]
        public async Task<IActionResult> CreateConnectedPairsAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateConnectedPairsAnswer), id);
        private async Task<IActionResult> GetViewToCreateQuestion(string actionName, int examId)
        {
            var exam = await examService.FindExamAsync(examId);
            if (exam == null) return NotFound();
            ViewData["TestId"] = exam.Id;
            return View(actionName);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTextAnswer(QuestionWithTextAnswerViewModel question) => await CreateQuestionWithAnswer(question);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel question) => await CreateQuestionWithAnswer(question);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSomeAnswer(QuestionWithSomeVariantsAnswerViewModel question) => await CreateQuestionWithAnswer(question);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSequenceAnswer(QuestionWithSequenceAnswerViewModel question) => await CreateQuestionWithAnswer(question);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConnectedPairsAnswer(QuestionWithConnectedPairsAnswerViewModel question) => await CreateQuestionWithAnswer(question);
        private async Task<IActionResult> CreateQuestionWithAnswer(QuestionWithAnswerViewModel question)
        {
            if(ModelState.IsValid)
            {
                await questionService.AddQuestionWithAnswerAsync(question);
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            else
            {
                ModelState.AddModelError("", "Присутні невірні данні");
                ViewData["TestId"] = question.ExamId;
                return View(question);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await questionService.FindQuestionWithAnswerAsync(id);
            if (question == null) return RedirectToAction("Index", "Tests");
            Type questionType = question.GetType();
            if(questionType == typeof(QuestionWithTextAnswerViewModel))
            {
                return View("EditTextAnswer");
            }
            else if(questionType == typeof(QuestionWithOneOfFourVariantsAnswerViewModel))
            {
                return View("EditOneOfFourAnswer");
            }else if(questionType == typeof(QuestionWithSomeVariantsAnswerViewModel))
            {
                return View("EditSomeVariantsAnswer");
            }else if(questionType == typeof(QuestionWithSequenceAnswerViewModel))
            {
                return View("EditSequenceAnswer");
            }
            else if(questionType == typeof(QuestionWithConnectedPairsAnswerViewModel))
            {
                return View("EditConnectedPairAnswer");
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTextAnswer(QuestionWithTextAnswerViewModel vm){
            await questionService.UpdateQuestionWithAnswerAsync(vm);
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel vm){
            await questionService.UpdateQuestionWithAnswerAsync(vm);
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSomeVariantsAnswer(QuestionWithSomeVariantsAnswerViewModel vm)
        {
            await questionService.UpdateQuestionWithAnswerAsync(vm);
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSequenceAnswer(QuestionWithSequenceAnswerViewModel vm)
        {
            await questionService.UpdateQuestionWithAnswerAsync(vm);
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConnectedPairAnswer(QuestionWithConnectedPairsAnswerViewModel vm)
        {
            await questionService.UpdateQuestionWithAnswerAsync(vm);
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var q = await questionService.FindQuestionWithAnswerAsync(id);
            if(q == null)return NotFound();
            return View(q);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ConfirmedDeleteForQuestionViewModel confirmedDelete)
        {
            await questionService.DeleteAsync(confirmedDelete.Id);
            return RedirectToAction("Details", "Tests", new { id = confirmedDelete.TestId });
        }
    }
}
