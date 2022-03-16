using System.Threading.Tasks;
using System.Collections.Generic;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces;

public interface ISessionService
{
    Task<List<SessionViewModel>> GetSessionListAsync();
    Task CloseSessionAsync(int id);
    Task CloseAllSessionsAsync();
}