using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.Models;

namespace UnitTests.ServicesTests;
#pragma warning disable
public class GroupServiceTests
{



    public class GetGroupListMethodTests:TestBase
    {
        public GetGroupListMethodTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void ReturnsAllGroupsFromDatabase()
        {
            using var context = CreateContext();
            var groupService = new GroupService(context);

            List<GroupViewModel> groups = await groupService.GetGroupListAsync();

            Assert.Equal(3, groups.Count);
            Assert.Collection(groups,
                i1 => Assert.Equal("group1", i1.Name),
                i2 => Assert.Equal("group2", i2.Name),
                i3 => Assert.Equal("group3", i3.Name)
            );
        }
        [Fact]
        public async void ReturnsEmptyList_WhenCountOfGroupsInDatabaseEquals0()
        {
            using var context = CreateTransactionalContext();
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            var groupService = new GroupService(context);

            List<GroupViewModel> groups = await groupService.GetGroupListAsync();

            Assert.NotNull(groups);
            Assert.Equal(0, groups.Count);
        }
    }





    public class FindMethodTests:TestBase
    {
        public FindMethodTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void FindsGroupById()
        {
            using var context = CreateContext();
            var groupService = new GroupService(context);
            Group someGroup = context.Groups.FirstOrDefault(g=>true);

            GroupViewModel group = await groupService.FindAsync(someGroup.Id);

            Assert.NotNull(group);
            Assert.IsType<GroupViewModel>(group);
        }
        [Fact]
        public async void ReturnsNull_WhenGroupDoesNotExistInDatabase()
        {
            using var context = CreateTransactionalContext();
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            GroupService service = new GroupService(context);

            GroupViewModel group = await service.FindAsync(2);

            Assert.Null(group);
        }
    }




    public class CreateMethodTests:TestBase
    {
        public CreateMethodTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void RecordsGroupInDatabase()
        {
            using var context = CreateTransactionalContext();
            GroupService service = new GroupService(context);
            GroupViewModel newGroup = new GroupViewModel{Name="newGroup"};

            await service.CreateAsync(newGroup);
            context.ChangeTracker.Clear();
            Group groupInDb = context.Groups.SingleOrDefault(g=>g.Name == "newGroup");

            Assert.NotNull(groupInDb);
            Assert.Equal("newGroup", groupInDb.Name);
        }
        [Fact]
        public async void ThrowsArgumentNullException_WhenArgumentIsNull()
        {
            using var context = CreateContext();
            GroupService service = new GroupService(context);
            var createAction = async ()=>await service.CreateAsync(null);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(createAction);

            Assert.Equal("Value cannot be null.", exception.Message);
        }
        [Fact]
        public async void ThrowsInvalidOperationException_WhenGroupNameIsNullOrEmpty()
        {
            using var context = CreateTransactionalContext();
            GroupService service = new GroupService(context);
            var createAction1 = async ()=>await service.CreateAsync(new GroupViewModel{Name=""});
            var createAction2 = async ()=>await service.CreateAsync(new GroupViewModel());

            InvalidOperationException exception1 = await Assert.ThrowsAsync<InvalidOperationException>(createAction1);
            InvalidOperationException exception2 = await Assert.ThrowsAsync<InvalidOperationException>(createAction2);

            Assert.Equal("The group must have name", exception1.Message);
            Assert.Equal("The group must have name", exception2.Message);
        }
    }




    public class UpdateMethodTests:TestBase
    {
        public UpdateMethodTests(TestDatabaseFixture fixture):base(fixture){}
        [Fact]
        public async void UpdateNameOfGroupInDatabase()
        {
            using var context = CreateTransactionalContext();
            GroupService service = new GroupService(context);
            Group group = context.Groups.Take(1).ToList()[0];

            await service.UpdateAsync(new GroupViewModel{
                Id=group.Id,
                Name="UpdatedName"
            });
            context.ChangeTracker.Clear();

            Assert.Equal("UpdatedName", (context.Groups.First(g=>g.Id==group.Id)).Name);
        }
        [Fact]
        public async void ThrowsArgumentNullException_WhenArgumentIsNull()
        {
            using var context = CreateTransactionalContext();
            GroupService service = new GroupService(context);
            
            var updateAction = async () => await service.UpdateAsync(null);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(updateAction);
        }
        [Fact]
        public async void ThrowsInvalidOperationException_WhenNameIsNullOrEmpty()
        {
            using var context = CreateTransactionalContext();
            GroupService service = new GroupService(context);
            Group group = context.Groups.Take(1).ToArray()[0];

            var updateAction1 = async () => await service.UpdateAsync(new GroupViewModel{
                Id = group.Id,
                Name = ""
            });
            var updateAction2 = async () => await service.UpdateAsync(new GroupViewModel{
                Id = group.Id
            });

            InvalidOperationException exception1 = await Assert.ThrowsAsync<InvalidOperationException>(updateAction1);
            InvalidOperationException exception2 = await Assert.ThrowsAsync<InvalidOperationException>(updateAction2);

            Assert.Equal("The group must have name", exception1.Message);
            Assert.Equal("The group must have name", exception2.Message);
        }
        [Fact]
        public async void ThrowsDbUpdateException_WhenGroupDoesNotExist()
        {
            using var context = CreateTransactionalContext();
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            GroupService service = new GroupService(context);

            var updateAction = async () => await service.UpdateAsync(new GroupViewModel{Id=1, Name="unexistingGroup"});

            DbUpdateException exception = await Assert.ThrowsAsync<DbUpdateException>(updateAction);

            Assert.Equal("Specified group doesn't exist in database", exception.Message);
        }
    }
}