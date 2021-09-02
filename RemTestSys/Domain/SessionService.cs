using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public class SessionService : ISessionService
    {
        public SessionService(ISessionsDbContext sessionDbContext, IStudentsDbContext studentsDbContext, ITestsDbContext testsDbContext, ISessionBuilder sessionBuilder)
        {
            _studentsDbContext = studentsDbContext ?? throw new ArgumentNullException(nameof(IStudentsDbContext));
            _sessionsDbContext = sessionDbContext ?? throw new ArgumentNullException(nameof(ISessionsDbContext));
            _testsDbContext = testsDbContext ?? throw new ArgumentNullException(nameof(ITestsDbContext));
            _sessionBuilder = sessionBuilder ?? throw new ArgumentNullException(nameof(ISessionBuilder));
        }

        private readonly IStudentsDbContext _studentsDbContext;
        private readonly ISessionsDbContext _sessionsDbContext;
        private readonly ITestsDbContext _testsDbContext;
        private readonly ISessionBuilder _sessionBuilder;
        public async Task<Session> StartOrContinueTest(string logId, int testId)
        {
            Student student = await _studentsDbContext.FindStudent(logId);
            if (student == null) throw new NullReferenceException(nameof(Student));
            Session session = await _sessionsDbContext.FindSession(student.Id, testId);
            if (session != null && (session.Finished)){
                await _sessionsDbContext.DeleteSession(session);
                session = null;
            } 
            if(session == null)
            {
                Test test = await _testsDbContext.GetTest(testId);
                List<AccessToTest> accessToTest = await _testsDbContext.GetAccessListToTestsForStudent(student.Id);
                if (!accessToTest.Exists(acc => acc.Test.Id == test.Id)) throw new Exception($"The student {student.Id} hasn't got access to test {test.Id}");
                session = await _sessionBuilder.Build(test, student);
                await _sessionsDbContext.AddSession(session);
            }
            return session;
        }

        public async Task<AnswerResult> Answer(string logId, int sessionId, object data)
        {
            Session session = await FindSessionFor(logId, sessionId);
            if (session == null) throw new Exception($"Student which is specified isn't owner for session {sessionId} or the session doesn't exist");
            AnswerResult answerResult;
            if (session.CurrentQuestion.Answer.IsMatch(data))
            {
                answerResult = new AnswerResult { IsRight = true, RightText = null };
            }
            else
            {
                answerResult = new AnswerResult { 
                    IsRight = false,
                    RightText = session.CurrentQuestion.Answer.RightText
                };
            }
            session.NextQuestion();
            await _sessionsDbContext.UpdateSession(session);
            return answerResult;
        }

        public async Task<Session> FindSessionFor(string logId, int sessionId)
        {
            Session session = await _sessionsDbContext.FindSession(sessionId);
            if(session!=null && session.Student.LogId == logId)
            {
                return session;
            }
            else
            {
                return null;
            }
        }
    }
}
