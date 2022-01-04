using Xunit;
using RemTestSys.Domain.Models;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace UnitTests
{
    public class StudentControllerTests
    {
        public StudentControllerTests(){
            contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseSqlite("Filename=Test.db")
                                    .Options;
        }

        private readonly DbContextOptions<AppDbContext> contextOptions;

        [Fact]
        public void TestTest(){
            Assert.True(false);
        }

        private void Seed(){
            using (var context = new AppDbContext(contextOptions)){
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //fill in db

                context.SaveChanges();
            }
        }
    }
}
