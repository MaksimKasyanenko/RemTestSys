using System.Threading.Tasks;
using System.Collections.Generic;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Interfaces;

public interface IExamAccessService
{
    Task<List<AccessToExamViewModel>> GetAccessListAsync();
    Task OpenCommonAccessAsync(int testId);
    Task OpenGroupAccessAsync(int groupId, int testId);
    Task OpenPersonAccessAsync(int studentId, int testId);
    Task CloseAllAccessesAsync();
    Task CloseCommonAccessAsync(int accessId);
    Task CloseGroupAccessAsync(int accessId);
    Task ClosePersonAccessAsync(int accessId);
}