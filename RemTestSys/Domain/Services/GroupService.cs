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

        public async Task<List<GroupVM>> GetGroupListAsync(){
            return await dbContext.Groups.Select(g => new GroupVM{Id=g.Id, Name = g.Name}).ToListAsync();
        }
    }
}