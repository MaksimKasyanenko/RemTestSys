using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface IExamsDbContext
    {
        Task<IEnumerable<Exam>> GetExams(Predicate<Exam> filter);
    }
}