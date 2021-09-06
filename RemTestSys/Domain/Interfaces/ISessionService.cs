using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionService
    {
        Task<Session> BeginOrContinue(string logId, int id);
        Task<AnswerResult> Answer(string logId, int sessionId, object data);
        Task<Session> GetSessionFor(int sessionId, string logId);
    }
}