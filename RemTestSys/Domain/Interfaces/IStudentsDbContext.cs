using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface IStudentsDbContext
    {
        Task<IEnumerable<Student>> GetStudents(Predicate<Student> filter);
        Task<IEnumerable<AccessToTest>> GetAccessesToTests(Predicate<AccessToTest> filter);
    }
}