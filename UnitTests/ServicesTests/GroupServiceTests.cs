using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Services;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.ViewModels;

namespace UnitTests.ServicesTests;

public class GroupServiceTests
{
    public class GetGroupListMethodTests:IClassFixture<TestDatabaseFixture>
    {
        public GetGroupListMethodTests(TestDatabaseFixture fixture) => this.fixture = fixture;
        private readonly TestDatabaseFixture fixture;
        [Fact]
        public async void GettingAllGroupsFromDatabase()
        {
            using var context = fixture.CreateContext();
            context.Groups.AddRange(
                new Group{Name = "group1"},
                new Group{Name = "group2"},
                new Group{Name = "group3"}
            );
            context.SaveChanges();
            context.ChangeTracker.Clear();
            var groupService = new GroupService(context);

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