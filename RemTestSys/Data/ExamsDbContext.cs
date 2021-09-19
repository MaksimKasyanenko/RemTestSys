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
        public Task<IEnumerable<Exam>> GetExams(Predicate<Exam> filter)
        {
            throw new NotImplementedException();!!!!!!!
        }
    }
}
