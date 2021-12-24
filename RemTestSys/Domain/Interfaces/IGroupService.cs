using RemTestSys.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces{
    public interface IGroupService{
        Task<List<GroupVM>> GetGroupListAsync();
    }
}