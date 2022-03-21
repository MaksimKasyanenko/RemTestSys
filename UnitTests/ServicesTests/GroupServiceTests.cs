using System.Collections.Generic;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;

namespace UnitTests.ServicesTests;

public class GroupServiceTests
{
    public class GetGroupListMethodTests:IClassFixture<TestDatabaseFixture>
    {
        public GetGroupListMethodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void ReturnsAllGroupsFromDatabase()
        {
            var groupService = new GroupService(fixture.CreateContext());

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
            var context = fixture.CreateTransactionalContext();
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
        
    }
}