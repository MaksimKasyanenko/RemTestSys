using System.Threading.Tasks;
using System.Collections.Generic;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.ViewModels.QuestionsWithAnswers;

namespace RemTestSys.Domain.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionViewModel>> GetQuestionsFromExamAsync(int examId);
    Task AddQuestionWithAnswerToAsync(QuestionWithAnswerViewModel questionViewModel);
}