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

        // GET: QuestionsController/Create
        public async Task<IActionResult> Create(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t=>t.Id==id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }

        // POST: QuestionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel questionViewModel)
        {
            if (!questionViewModel.IsValid)
            {
                ModelState.AddModelError("", "Присутні невірні данні");
                return View(questionViewModel);
            }
            else
            {
                Question question = questionViewModel.GetQuestion();
                dbContext.Questions.Add(question);
                await dbContext.SaveChangesAsync();
                Answer answer = questionViewModel.GetAnswer();
                answer.Question = question;
                await answer.ToDb(dbContext);
            }
            return RedirectToAction("Details", "Tests", new {id=questionViewModel.TestId});
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
