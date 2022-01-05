using System;
using RemTestSys.Domain;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class TestClassWithDb
    {
        public TestClassWithDb(DbContextOptions<AppDbContext> options){
            ContextOptions = options;
        }

        protected readonly DbContextOptions<AppDbContext> ContextOptions;

        protected void SeedDb(Action<AppDbContext> seed){
            using (var context = new AppDbContext(ContextOptions)){
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                seed.Invoke(context);
                context.SaveChanges();
            }
        }
    }
}
