using System;
using RemTestSys.Domain;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class TestClassWithDb
    {
        public TestClassWithDb(DbContextOptions<AppDbContext> options){
            contextOptions = options;
        }

        private readonly DbContextOptions<AppDbContext> contextOptions;

        protected void SeedDb(Action<AppDbContext> seed){
            using (var context = new AppDbContext(contextOptions)){
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                seed.Invoke(context);
                context.SaveChanges();
            }
        }
    }
}
