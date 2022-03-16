using RemTestSys.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces{
    public interface IGroupService{
        Task<List<GroupViewModel>> GetGroupListAsync();
        Task<GroupViewModel> FindAsync(int id);
        Task CreateAsync(GroupViewModel group);
        Task UpdateAsync(GroupViewModel group);
        Task DeleteAsync(int id);
        bool Exists(int id);
    }
}