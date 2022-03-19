using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class TestsController : Controller
    {
        public TestsController(IExamService examService, IQuestionService questionService)
        {
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
            this.questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
        }
        private readonly IExamService examService;
        private readonly IQuestionService questionService;
        public async Task<IActionResult> Index()
        {
            return View(await examService.GetExamsAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            var exam = await examService.FindExamAsync(id);
            if (exam == null)return NotFound();
            ViewBag.QuestionsInExam = await questionService.GetQuestionsFromExamAsync(id);
            return View(exam);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamViewModel exam)
        {
            if (ModelState.IsValid)
            {
                await examService.CreateExamAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var exam = await examService.FindExamAsync(id);
            if(exam == null)return NotFound();
            return View(exam);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                await examService.UpdateExamAsync(examViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(examViewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await examService.FindExamAsync(id);
            if(exam == null)return NotFound();
            return View(exam);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await examService.DeleteExamAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
