using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.ViewModels.QuestionsWithAnswers;

namespace RemTestSys.Domain.Services;

public class QuestionService : IQuestionService
{
    public QuestionService(AppDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    private readonly AppDbContext dbContext;
    public async Task<IEnumerable<QuestionViewModel>> GetQuestionsFromExamAsync(int examId)
    {
        return await dbContext.Questions
                              .Where(q => q.TestId == examId)
                              .Select(q => new QuestionViewModel{
                                  Id = q.Id,
                                  Text = q.Text,
                                  SubText = q.SubText,
                                  RightAnswer = q.Answer.RightText,
                                  Cost = q.Cast
                              }).ToArrayAsync();
    }
    public async Task<QuestionWithAnswerViewModel> FindQuestionWithAnswerAsync(int id)
    {
        Question question = await dbContext.Questions.Include(q=>q.Answer).Include(q=>q.Test).FirstOrDefaultAsync(q=>q.Id == id);
        if(question == null)return null;
        Type answerType = question.Answer.GetType();
            if(answerType == typeof(TextAnswer))
            {
                return QuestionViewModel.CreateForTextAnswer(question);
            }
            else if(answerType == typeof(OneOfFourVariantsAnswer))
            {
                return QuestionViewModel.CreateForOneOfFourAnswer(question);
            }else if(answerType == typeof(SomeVariantsAnswer))
            {
                return QuestionViewModel.CreateForSomeVariantsAnswer(question);
            }else if(answerType == typeof(SequenceAnswer))
            {
                return QuestionViewModel.CreateForSequenceAnswer(question);
            }
            else if(answerType == typeof(ConnectedPairsAnswer))
            {
                return QuestionViewModel.CreateForConnectedPairAnswer(question);
            }
            throw new NotImplementedException(answerType.FullName);
    }
    public async Task AddQuestionWithAnswerToAsync(QuestionWithAnswerViewModel questionViewModel)
    {
        Type questionType = questionViewModel.GetType();
        if(questionType == typeof(QuestionWithTextAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithTextAnswerViewModel;
            await Create(q, () => Answer.CreateTextAnswer(q.RightText, q.CaseMatters));
        }
        else if(questionType == typeof(QuestionWithOneOfFourVariantsAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithOneOfFourVariantsAnswerViewModel;
            await Create(q, () => Answer.CreateOneOfFourVariantsAnswer(q.RightVariant, q.Fake1, q.Fake2,q.Fake3));
        }
        else if(questionType == typeof(QuestionWithSomeVariantsAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithSomeVariantsAnswerViewModel;
            await Create(q, () => Answer.CreateSomeVariantsAnswer(q.RightVariants, q.FakeVariants));
        }
        else if(questionType == typeof(QuestionWithSequenceAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithSequenceAnswerViewModel;
            await Create(q, () => Answer.CreateSequenceAnswer(q.Sequence));
        }
        else if(questionType == typeof(QuestionWithConnectedPairsAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithConnectedPairsAnswerViewModel;
            await Create(q, () => Answer.CreateConnectedPairsAnswer(q.LeftList, q.RightList));
        }
        else
        {
            throw new NotSupportedException($"{questionType.FullName} type is not supported");
        }
    }
    private async Task Create(QuestionViewModel question, Func<Answer> getAnswerModel)
        {
            Question ques = Question.Create(question.Text, question.SubText, question.ExamId, question.Cost);
            Answer answ  = getAnswerModel();
            dbContext.Questions.Add(ques);
            answ.Question = ques;
            dbContext.Add(answ);
            await dbContext.SaveChangesAsync();
        }
}