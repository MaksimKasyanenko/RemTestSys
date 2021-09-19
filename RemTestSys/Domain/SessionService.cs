using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.Exceptions;

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
        public async Task<Session> BeginOrContinue(string logId, int testId)
        {
            Student student = (await _studentsDbContext.GetStudents(st=>st.LogId == logId)).FirstOrDefault();
            if (student == null) throw new NotExistException("The student for specified LogId do not exists");
            Session session = (await _sessionsDbContext.GetSessions(s => s.Student.LogId == logId && s.Test.Id == testId)).SingleOrDefault();
            if (session != null && (session.Finished)){
                await _sessionsDbContext.DeleteSession(session);
                session = null;
            } 
            if(session == null)
            {
                Test test = await _testsDbContext.GetTest(testId);
                AccessToTest accessToTest = (await _studentsDbContext.GetAccessesToTests(at => at.Test.Id == testId && at.Student.Id == student.Id)).FirstOrDefault();
                if (accessToTest == null) throw new DataAccessException($"The student {student.Id} hasn't got access to test {test.Id}");
                session = _sessionBuilder.Build(test, student);
                session.StartTime = DateTime.Now;
                await _sessionsDbContext.AddSession(session);
            }
            return session;
        }

        public async Task<AnswerResult> Answer(string logId, int sessionId, object data)
        {
            Session session = (await _sessionsDbContext.GetSessions(s => s.Id == sessionId && s.Student.LogId == logId)).SingleOrDefault();
            if (session == null) throw new NotExistException($"Student which is specified isn't owner for session {sessionId} or the session doesn't exist");
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

        public async Task<Session> GetSessionFor(int sessionId, string logId)
        {
            Session session = (await _sessionsDbContext.GetSessions(s => s.Id == sessionId && s.Student.LogId == logId)).FirstOrDefault();
            if (session == null) throw new DataAccessException($"The student for specified LogId havn't got access to session({sessionId}) or the session doesn't exist");
            return session;
        }
    }
}
