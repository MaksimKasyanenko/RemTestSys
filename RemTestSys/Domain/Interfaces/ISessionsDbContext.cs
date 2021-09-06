using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionsDbContext
    {
        Task DeleteSession(Session session);
        Task AddSession(Session session);
        Task UpdateSession(Session session);
        Task<IEnumerable<Session>> GetSessions(Predicate<Session> filter);
    }
}