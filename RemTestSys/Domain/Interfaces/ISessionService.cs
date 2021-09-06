using RemTestSys.Domain.Models;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionService
    {
        Task<Session> BeginOrContinue(string logId, int testId);
        Task<AnswerResult> Answer(string logId, int sessionId, object data);
        Task<Session> GetSessionFor(int sessionId, string logId);
    }
}