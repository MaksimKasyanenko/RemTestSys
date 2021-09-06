using System.Threading.Tasks;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionBuilder
    {
        Task<Session> Build(Test test, Student student);
    }
}