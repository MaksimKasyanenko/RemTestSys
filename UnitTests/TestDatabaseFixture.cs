using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain;

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
                    context.Database.EnsureCreated();
                    FillDb();
                }

                _databaseInitialized = true;
            }
        }
    }

    public AppDbContext CreateContext()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(ConnectionString).Options);
        context.Database.BeginTransaction();
        return context;
    }
    public void FillDb()
    {

    }
}