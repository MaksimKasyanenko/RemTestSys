using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Data
{
    public class StudentsDbContext : IStudentsDbContext
    {
        public StudentsDbContext(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext _appDbContext;

        public async Task<IEnumerable<Student>> GetStudents(Predicate<Student> filter)
        {
            return await _appDbContext.Students.Where(st => filter(st)).ToArrayAsync();
        }

        public async Task<IEnumerable<AccessToTest>> GetAccessesToTests(Predicate<AccessToTest> filter)
        {
            return await _appDbContext.AccessesToTest.Where(at => filter(at)).ToArrayAsync();
        }
    }
}
