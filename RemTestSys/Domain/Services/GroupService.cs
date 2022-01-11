using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Domain.Services{
    public class GroupService : IGroupService{

        public GroupService(AppDbContext dbContext){
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private readonly AppDbContext dbContext;

        public async Task<List<GroupViewModel>> GetGroupListAsync(){
            return await dbContext.Groups.Select(g => new GroupViewModel{Id=g.Id, Name = g.Name}).ToListAsync();
        }
    }
}