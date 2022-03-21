using System.Collections.Generic;
using Xunit;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.ViewModels;

namespace UnitTests.ServicesTests;

public class GroupServiceTests
{
    public class GetGroupListMethodTests
    {
        public GetGroupListMethodTests() => this.fixture = new TestDatabaseFixture();
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void GettingAllGroupsFromDatabase()
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
    }
}