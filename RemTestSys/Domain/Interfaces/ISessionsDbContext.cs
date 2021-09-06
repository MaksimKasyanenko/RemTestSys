using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionsDbContext
    {
        Task<Session> FindSession(int id, int testId);
        Task DeleteSession(Session session);
        Task AddSession(Session session);
        Task UpdateSession(Session session);
        Task<Session> FindSession(int sessionId);
    }
}