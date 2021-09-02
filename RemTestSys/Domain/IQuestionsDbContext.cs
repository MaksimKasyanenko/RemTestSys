using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface IQuestionsDbContext
    {
        Task<List<Question>> GetQuestionsForTest(int id);
    }
}