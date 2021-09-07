using RemTestSys.Domain.Models;

namespace RemTestSys.Domain.Interfaces
{
    public interface ISessionBuilder
    {
        Session Build(Test test, Student student);
    }
}