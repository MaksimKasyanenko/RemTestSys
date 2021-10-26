using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain.Models;
using RemTestSys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class QuestionsController : Controller
    {
        public QuestionsController(AppDbContext context)
        {
            dbContext = context;
        }
        private readonly AppDbContext dbContext;

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            return await CreateTextAnswer(id);
        }



        [HttpGet]
        public async Task<IActionResult> CreateTextAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View("CreateTextAnswer");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTextAnswer(QuestionWithTextAnswerViewModel question)
        {
            Question ques;
            Answer answ;
            try
            {
                ques = Question.Create(question.Text, question.SubText, question.TestId);
                answ = Answer.CreateTextAnswer(question.RightText, question.CaseMatters);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("", "Присутні невірні данні");
                ViewData["TestId"] = question.TestId;
                return View(question);
            }
            dbContext.Questions.Add(ques);
            await dbContext.SaveChangesAsync();
            answ.Question = ques;
            await answ.ToDb(dbContext);
            return RedirectToAction("Details", "Tests", new { id = question.TestId });
        }




        // GET: QuestionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuestionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Question q = await dbContext.Questions.Where(q => q.Id == id).Include(q=>q.Answer).SingleAsync();
            return View(q);
        }

        // POST: QuestionsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ConfirmedDeleteForQuestionViewModel confirmedDelete)
        {
            var question = await dbContext.Questions.FindAsync(confirmedDelete.Id);
            dbContext.Questions.Remove(question);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Details", "Tests", new { id=confirmedDelete.TestId});
        }
    }
}
