using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Exceptions;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Data
{
    public class SessionDbContext : ISessionsDbContext
    {
        public SessionDbContext(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext _appDbContext;
        public async Task AddSession(Session session)
        {
            _appDbContext.Sessions.Add(session);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteSession(int id)
        {
            Session s = _appDbContext.Sessions.Single(s => s.Id == id);
            _appDbContext.Sessions.Remove(s);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Session>> GetSessions(Predicate<Session> filter)
        {
            return await _appDbContext.Sessions.Where(s => filter(s)).ToArrayAsync();
        }

        public async Task UpdateSession(Session session)
        {
            Session s = _appDbContext.Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (s == null) throw new DataAccessException("trying to update non-existent session");
            _appDbContext.Update(session);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
