using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

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
}