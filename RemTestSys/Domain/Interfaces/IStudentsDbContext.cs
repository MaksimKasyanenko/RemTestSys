using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface IStudentsDbContext
    {
        Task<Student> FindStudent(string logId);
        Task<IEnumerable<AccessToTest>> GetAccessesToTests(Predicate<AccessToTest> filter);
    }
}