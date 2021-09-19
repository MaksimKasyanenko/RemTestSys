using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RemTestSys.Data
{
    public class ExamsDbContext : IExamsDbContext
    {
        public ExamsDbContext(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }
        private readonly AppDbContext _appDbContext;
        public async Task<IEnumerable<Exam>> GetExams(Predicate<Exam> filter)
        {
            return await _appDbContext.Exams.Where(e => filter(e)).ToArrayAsync();
        }
    }
}
