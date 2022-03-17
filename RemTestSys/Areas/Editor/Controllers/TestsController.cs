using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
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
            var test = await _context.Tests.Where(t => t.Id == id).Include(t => t.MapParts).SingleOrDefaultAsync();
            if (test == null)
            {
                return NotFound();
            }
            return View(new TestViewModel {
                TestId=test.Id,
                Name=test.Name,
                Description=test.Description,
                Duration=test.Duration,
                MapParts=test.MapParts.ToArray()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                Test test = await _context.Tests.Where(t=>t.Id == testViewModel.TestId).Include(t=>t.MapParts).SingleOrDefaultAsync();
                if (test == null) return NotFound();
                _context.MapParts.RemoveRange(test.MapParts.ToArray());
                await _context.SaveChangesAsync();
                test.Name = testViewModel.Name;
                test.Description = testViewModel.Description;
                test.Duration = testViewModel.Duration;
                test.MapParts = testViewModel.MapParts;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testViewModel);
        }

        // GET: Editor/Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Editor/Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
