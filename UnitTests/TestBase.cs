using System;
using Xunit;
using RemTestSys.Domain;
namespace UnitTests;
public class TestBase:IClassFixture<TestDatabaseFixture>
{
    public TestBase(TestDatabaseFixture fixture)
    {
        this.fixture = fixture ?? throw new ArgumentNullException("The fixture wasn't passed to base class");
    }
    private readonly TestDatabaseFixture fixture;
    protected AppDbContext CreateContext() => fixture.CreateContext();
    protected AppDbContext CreateTransactionalContext() => fixture.CreateTransactionalContext();
}