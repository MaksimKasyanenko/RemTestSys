using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RemTestSys.Data
{
    public class TestsDbContext : ITestsDbContext
    {
        public TestsDbContext(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext _appDbContext;
        public async Task<IEnumerable<Test>> GetTests(Predicate<Test> filter)
        {
            return await _appDbContext.Tests.Where(t => filter(t)).ToArrayAsync();
        }
    }
}
