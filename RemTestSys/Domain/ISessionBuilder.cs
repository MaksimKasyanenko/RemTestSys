using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public interface ISessionBuilder
    {
        Task<Session> Build(Test test, Student student);
    }
}