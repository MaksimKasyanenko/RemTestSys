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

        public GroupService(AppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private readonly AppDbContext dbContext;

        public async Task<List<GroupViewModel>> GetGroupListAsync()
        {
            return await dbContext.Groups.Select(g => new GroupViewModel{Id=g.Id, Name = g.Name}).ToListAsync();
        }
        public async Task<GroupViewModel> FindAsync(int id)
        {
            var group = await dbContext.Groups.SingleAsync(g => g.Id == id);
            if(group == null)return null;
            return new GroupViewModel{
                Id = group.Id,
                Name = group.Name
            };
        }
        public async Task CreateAsync(GroupViewModel group)
        {
            dbContext.Groups.Add(new Group{Name = group.Name});
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(GroupViewModel group)
        {
            if(!Exists(group.Id))throw new DbUpdateException("It's impossible to update entity that doesn't exist");
            dbContext.Groups.Update(new Group{
                Id = group.Id,
                Name = group.Name
            });
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if(!Exists(id))throw new DbUpdateException("It's impossible to delete entity that doesn't exist");
            dbContext.Groups.Remove(await dbContext.Groups.SingleAsync(g=>g.Id==id));
            await dbContext.SaveChangesAsync();
        }
        public bool Exists(int id)
        {
            return dbContext.Groups.Any(g => g.Id == id);
        }
        public async Task<List<StudentViewModel>> GetStudentsForGroupAsync(int id)
        {
            var group = await dbContext.Groups.Where(g=>g.Id == id).Include(g=>g.Students).FirstAsync();
            return group.Students.Select(s => new StudentViewModel{
                                             Id = s.Id,
                                             FirstName = s.FirstName,
                                             LastName = s.LastName,
                                             GroupId = s.GroupId,
                                             GroupName = group.Name,
                                             RegistrationDate = s.RegistrationDate
                                         }).ToList();
        }
    }
}