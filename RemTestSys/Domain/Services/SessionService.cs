using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Services;

public class SessionService : ISessionService
{
    public SessionService(AppDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    private readonly AppDbContext dbContext;
    public async Task<List<SessionViewModel>> GetSessionListAsync()
    {
        return await dbContext.Sessions
                              .Select(s => new SessionViewModel{
                                  Id = s.Id,
                                  TestName = s.Test.Name,
                                  StudentName = s.Student.FullName,
                                  StartTime = s.StartTime,
                                  Finished = s.Finished
                              }).ToListAsync();
    }
    public async Task CloseSessionAsync(int id)
    {
        var session = dbContext.Sessions.FirstOrDefault(s => s.Id == id);
        if(session != null)
        {
            dbContext.Sessions.Remove(session);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task CloseAllSessionsAsync()
    {
        dbContext.Sessions.RemoveRange(dbContext.Sessions);
        await dbContext.SaveChangesAsync();
    }
}