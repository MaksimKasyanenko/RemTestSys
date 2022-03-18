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
        public QuestionsController(IExamService examService, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.examService = examService ?? throw new ArgumentNullException(nameof(examService));
        }
        private readonly IExamService examService;
        private readonly AppDbContext dbContext;
        [HttpGet]
        public async Task<IActionResult> CreateTextAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateTextAnswer), id);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTextAnswer(QuestionWithTextAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateTextAnswer(question.RightText, question.CaseMatters)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            return View(question);
        }


        [HttpGet]
        public async Task<IActionResult> CreateOneOfFourAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateOneOfFourAnswer), id);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateOneOfFourVariantsAnswer(question.RightVariant, question.Fake1, question.Fake2, question.Fake3)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSomeAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateSomeAnswer), id);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSomeAnswer(QuestionWithSomeVariantsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateSomeVariantsAnswer(question.RightVariants, question.FakeVariants)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSequenceAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateSequenceAnswer), id);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSequenceAnswer(QuestionWithSequenceAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateSequenceAnswer(question.Sequence)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> CreateConnectedPairsAnswer(int id) => await GetViewToCreateQuestion(nameof(CreateConnectedPairsAnswer), id);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConnectedPairsAnswer(QuestionWithConnectedPairsAnswerViewModel question)
        {
            if (await TryCreateAnswer(question, () => Answer.CreateConnectedPairsAnswer(question.LeftList,question.RightList)))
            {
                return RedirectToAction("Details", "Tests", new { id = question.ExamId });
            }
            return View(question);
        }
        private async Task<IActionResult> GetViewToCreateQuestion(string actionName, int examId)
        {
            var exam = await examService.FindExamAsync(examId);
            if (exam == null) return NotFound();
            ViewData["TestId"] = exam.Id;
            return View(actionName);
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
                return View("EditSomeVariantsAnswer", QuestionViewModel.CreateForSomeVariantsAnswer(question));
            }else if(answerType == typeof(SequenceAnswer))
            {
                return View("EditSequenceAnswer", QuestionViewModel.CreateForSequenceAnswer(question));
            }
            else if(answerType == typeof(ConnectedPairsAnswer))
            {
                return View("EditConnectedPairAnswer", QuestionViewModel.CreateForConnectedPairAnswer(question));
            }
            throw new NotImplementedException(answerType.FullName);
        }

        [HttpPost]
        public async Task<IActionResult> EditTextAnswer(QuestionWithTextAnswerViewModel vm){
            Question question = await dbContext.Questions
                                         .Where(q=>q.Id==vm.Id)
                                         .Include(q=>q.Answer)
                                         .SingleOrDefaultAsync();
            if(question!=null){
                question.Text=vm.Text;
                question.SubText=vm.SubText;
                question.Cast=vm.Cost;
                question.Answer.RightText=vm.RightText;
                ((TextAnswer)question.Answer).CaseMatters=vm.CaseMatters;
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
            
        }
        [HttpPost]
        public async Task<IActionResult> EditOneOfFourAnswer(QuestionWithOneOfFourVariantsAnswerViewModel vm){
            Question question = await dbContext.Questions
                                         .Where(q=>q.Id==vm.Id)
                                         .Include(q=>q.Answer)
                                         .SingleOrDefaultAsync();
            if(question!=null){
                ((OneOfFourVariantsAnswer)question.Answer).SetFakes(vm.Fake1, vm.Fake2, vm.Fake3);
                question.Text=vm.Text;
                question.SubText=vm.SubText;
                question.Cast=vm.Cost;
                question.Answer.RightText = vm.RightVariant;
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details","Tests",new {id=vm.ExamId});
        }
        [HttpPost]
        public async Task<IActionResult> EditSomeVariantsAnswer(QuestionWithSomeVariantsAnswerViewModel vm)
        {
            Question question = await dbContext.Questions
                                         .Where(q => q.Id == vm.Id)
                                         .Include(q => q.Answer)
                                         .SingleOrDefaultAsync();
            if (question != null)
            {
                question.Text = vm.Text;
                question.SubText = vm.SubText;
                question.Cast = vm.Cost;
                ((SomeVariantsAnswer)question.Answer).SetRightAnswers(vm.RightVariants);
                ((SomeVariantsAnswer)question.Answer).SetFakes(vm.FakeVariants);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Tests", new { id = vm.ExamId });

        }
        [HttpPost]
        public async Task<IActionResult> EditSequenceAnswer(QuestionWithSequenceAnswerViewModel vm)
        {
            Question question = await dbContext.Questions
                                         .Where(q => q.Id == vm.Id)
                                         .Include(q => q.Answer)
                                         .SingleOrDefaultAsync();
            if (question != null)
            {
                question.Text = vm.Text;
                question.SubText = vm.SubText;
                question.Cast = vm.Cost;
                ((SequenceAnswer)question.Answer).SetSequence(vm.Sequence);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Tests", new { id = vm.ExamId });
        }
        [HttpPost]
        public async Task<IActionResult> EditConnectedPairAnswer(QuestionWithConnectedPairsAnswerViewModel vm)
        {
            Question question = await dbContext.Questions
                                         .Where(q => q.Id == vm.Id)
                                         .Include(q => q.Answer)
                                         .SingleOrDefaultAsync();
            if (question != null)
            {
                question.Text = vm.Text;
                question.SubText = vm.SubText;
                question.Cast = vm.Cost;
                ConnectedPairsAnswer.Pair[] pairs = new ConnectedPairsAnswer.Pair[vm.LeftList.Length];
                for(int i =0; i < pairs.Length; i++)
                {
                    pairs[i] = new ConnectedPairsAnswer.Pair { Value1 = vm.LeftList[i], Value2 = vm.RightList[i] };
                }
                ((ConnectedPairsAnswer)question.Answer).SetPairs(pairs);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Tests", new { id = vm.ExamId });
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
                ques = Question.Create(question.Text, question.SubText, question.ExamId, question.Cost);
                answ = getAnswerModel();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("", "Присутні невірні данні");
                ViewData["TestId"] = question.ExamId;
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
