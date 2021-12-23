using System.Collections.Generic;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Services{
    public class GroupService{

        public GroupService(AppDbContext dbContext){
            this.dbContext = bdContext ?? throw new ArgumentNullReferenceException(nameof(dbContext));
        }

        private readonly AppDbContext dbContext;

        public async Task<List<Group>> GetGroupList(){
            await dbContext.Groups.Select(g => new GroupVM{Id=g.Id, Name = g.Name}).ToListAsync();
        }
    }
}