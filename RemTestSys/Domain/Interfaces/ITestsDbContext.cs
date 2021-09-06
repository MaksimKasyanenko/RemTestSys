using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ITestsDbContext
    {
        Task<Test> GetTest(int testId);
    }
}
