using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using RemTestSys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Editor")]
    public class QuestionsController : Controller
    {
        public QuestionsController(AppDbContext context)
        {
            dbContext = context;
        }
        private readonly AppDbContext dbContext;

        [HttpGet]
        public async Task<IActionResult> CreateTextAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTextAnswer(QuestionWithTextAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateTextAnswer(question.RightText, question.CaseMatters)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            }
            return View(question);
        }


        [HttpGet]
        public async Task<IActionResult> CreateOneOfFourAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateOneOfFourVariantsAnswer(question.RightVariant, question.Fake1, question.Fake2, question.Fake3)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSomeAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSomeAnswer(QuestionWithSomeVariantsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateSomeVariantsAnswer(question.RightVariants, question.FakeVariants)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSequenceAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSequenceAnswer(QuestionWithSequenceAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateSequenceAnswer(question.Sequence)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateConnectedPairsAnswer(int id)
        {
            Test test = await dbContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            ViewData["TestId"] = test.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConnectedPairsAnswer(QuestionWithConnectedPairsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateConnectedPairsAnswer(question.LeftList,question.RightList)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            }
            return View(question);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Question question = await dbContext.Questions
                                               .Include(q=>q.Answer)
                                               .Include(q=>q.Test)
                                               .SingleOrDefaultAsync(q=>q.Id==id);
            if (question == null) return RedirectToAction("Index", "Tests");
            Type answerType = question.Answer.GetType();
            if(answerType == typeof(TextAnswer))
            {
                return View("EditTextAnswer", QuestionViewModel.CreateForTextAnswer(question));
            }
            else if(answerType == typeof(OneOfFourVariantsAnswer))
            {
                return View("EditOneOfFourAnswer", QuestionViewModel.CreateForOneOfFourAnswer(question));
            }else if(answerType == typeof(SomeVariantsAnswer))
            {
                return View(QuestionViewModel.CreateForSomeVariantsAnswer(question));
            }else if(answerType == typeof(SequenceAnswer))
            {
                return View(QuestionViewModel.CreateForSequenceAnswer(question));
            }
            else if(answerType == typeof(ConnectedPairsAnswer))
            {
                return View(QuestionViewModel.CreateForConnectedPairAnswer(question));
            }
            throw new NotImplementsException(answerType.FullName);
        }
        [HttpPost]
        public async Task<IActionResult> EditTextAnswer(QuestionWithTextAnswerViewModel vm){
            Question question = await dbContext.Questions
                                         .Where(q=>q.Id==vm.QuestionId)
                                         .Include(q=>q.Answer)
                                         .SingleOrDefaultAsync();
            if(question!=null){
                question.Text=vm.QuestionText;
                question.SubText=vm.QuestionSubText;
                question.Answer.RightText=vm.RightText;
                question.Answer.CaseMatters=vm.CaseMatters;
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details","Tests",new {id=vm.TestId});
            
        }
        [HttpPost]
        public async Task<IActionResult> EditOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel){
            Question question = await dbContext.Questions
                                         .Where(q=>q.Id==vm.QuestionId)
                                         .Include(q=>q.Answer)
                                         .SingleOrDefaultAsync();
            if(question!=null){
                question.Text=vm.QuestionText;
                question.SubText=vm.QuestionSubText;
                question.Answer.RightText = vm.RightVariant;
                question.Answer.SetFakes(vm.Fake1, vm.Fake2, vm.Fake3);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details","Tests",new {id=vm.TestId});
        }
        public async Task<IActionResult> Delete(int id)
        {
            Question q = await dbContext.Questions.Where(q => q.Id == id).Include(q => q.Answer).SingleAsync();
            return View(q);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ConfirmedDeleteForQuestionViewModel confirmedDelete)
        {
            var question = await dbContext.Questions.FindAsync(confirmedDelete.Id);
            dbContext.Questions.Remove(question);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Details", "Tests", new { id = confirmedDelete.TestId });
        }
        private async Task<bool> TryCreateAnswer(QuestionViewModel question, Func<Answer> getAnswerModel)
        {
            Question ques;
            Answer answ;
            try
            {
                ques = Question.Create(question.Text, question.SubText, question.TestId);
                answ = getAnswerModel();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("", "Присутні невірні данні");
                ViewData["TestId"] = question.TestId;
                return false;
            }
            dbContext.Questions.Add(ques);
            answ.Question = ques;
            dbContext.Add(answ);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
