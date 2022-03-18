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
    public async Task AddQuestionWithAnswerToAsync(QuestionWithAnswerViewModel questionViewModel)
    {
        if(questionViewModel.GetType() == typeof(QuestionWithTextAnswerViewModel))
        {
            var q = questionViewModel as QuestionWithTextAnswerViewModel;
            await Create(q, () => Answer.CreateTextAnswer(q.RightText, q.CaseMatters));
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