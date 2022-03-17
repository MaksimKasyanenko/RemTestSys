using System.Threading.Tasks;
using System.Collections.Generic;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces;

public interface IQuestionService
{
    public Task<IEnumerable<QuestionViewModel>> GetQuestionsFromExamAsync(int examId);
}