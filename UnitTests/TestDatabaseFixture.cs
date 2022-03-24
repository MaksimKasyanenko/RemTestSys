using System.Linq;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;

namespace UnitTests;
public class TestDatabaseFixture
{
    private const string ConnectionString = @"Server=(localdb)\himsqlins;Initial Catalog=himtestdb;Integrated Security=true;";
    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                    FillDb();
                }

                _databaseInitialized = true;
            }
        }
    }

    public AppDbContext CreateContext()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(ConnectionString).Options);
        return context;
    }
    public AppDbContext CreateTransactionalContext()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(ConnectionString).Options);
        context.Database.BeginTransaction();
        return context;
    }
    private void FillDb()
    {
        using var context = CreateContext();
        context.Groups.AddRange(
                new Group{Name = "group1"},
                new Group{Name = "group2"},
                new Group{Name = "group3"}
            );
        context.SaveChanges();

        Group group = context.Groups.First(g=>g.Name=="group3");
        context.Students.AddRange(
            new Student{FirstName = "student1", Group = group, LogId="12345678"},
            new Student{FirstName = "student2", Group = group, LogId="87654321"}
        );
        context.SaveChanges();
    }
}