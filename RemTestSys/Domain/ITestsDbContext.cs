using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface ITestsDbContext
    {
        Task<Test> GetTest(int testId);
        Task<List<AccessToTest>> GetAccessListToTestsForStudent(int id);
    }
}
