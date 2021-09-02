using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface ISessionService
    {
        Task<Session> StartOrContinueTest(string logId, int id);
        Task<AnswerResult> Answer(string logId, int sessionId, object data);
        Task<Session> FindSessionFor(string logId, int sessionId);
    }
}