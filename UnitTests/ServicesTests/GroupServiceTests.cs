using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;
using RemTestSys.Domain.Models;

namespace UnitTests.ServicesTests;
#pragma warning disable
public class GroupServiceTests
{
    public class GetGroupListMethodTests:IClassFixture<TestDatabaseFixture>
    {
        public GetGroupListMethodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void ReturnsAllGroupsFromDatabase()
        {
            using var context = fixture.CreateContext();
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
            using var context = fixture.CreateTransactionalContext();
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            var groupService = new GroupService(context);

            List<GroupViewModel> groups = await groupService.GetGroupListAsync();

            Assert.NotNull(groups);
            Assert.Equal(0, groups.Count);
        }
    }

    public class FindMethodTests:IClassFixture<TestDatabaseFixture>
    {
        public FindMethodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void FindsGroupById()
        {
            using var context = fixture.CreateContext();
            var groupService = new GroupService(context);
            Group someGroup = context.Groups.FirstOrDefault(g=>true);

            GroupViewModel group = await groupService.FindAsync(someGroup.Id);

            Assert.NotNull(group);
            Assert.IsType<GroupViewModel>(group);
        }
        [Fact]
        public async void ReturnsNull_WhenGroupDoesNotExistInDatabase()
        {
            using var context = fixture.CreateTransactionalContext();
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            GroupService service = new GroupService(context);

            GroupViewModel group = await service.FindAsync(2);

            Assert.Null(group);
        }
    }
    public class CreateMethodTests:IClassFixture<TestDatabaseFixture>
    {
        public CreateMethodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void RecordsGroupInDatabase()
        {
            using var context = fixture.CreateTransactionalContext();
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
            using var context = fixture.CreateContext();
            GroupService service = new GroupService(context);
            var createAction = async ()=>await service.CreateAsync(null);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(createAction);

            Assert.Equal("Value cannot be null.", exception.Message);
        }
        [Fact]
        public async void ThrowsOperationException_WhenGroupNameIsNullOrEmpty()
        {
            using var context = fixture.CreateTransactionalContext();
            GroupService service = new GroupService(context);
            var createAction1 = async ()=>await service.CreateAsync(new GroupViewModel{Name=""});
            var createAction2 = async ()=>await service.CreateAsync(new GroupViewModel());

            InvalidOperationException exception1 = await Assert.ThrowsAsync<InvalidOperationException>(createAction1);
            InvalidOperationException exception2 = await Assert.ThrowsAsync<InvalidOperationException>(createAction2);
        }
    }
    public class UpdateMetthodTests:IClassFixture<TestDatabaseFixture>
    {
        public UpdateMetthodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        
    }
}