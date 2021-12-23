using System.Collections.Generic;

namespace RemTestSys.Domain.Interfaces{
    public interface IGroupService{
        Task<List<GroupVM>> GetGroupListAsync();
    }
}