using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using RemTestSys.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace RemTestSys.Domain
{
    public class SessionService : ISessionService
    {
        public SessionService(AppDbContext appDbContext, ISessionBuilder sessionBuilder)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _sessionBuilder = sessionBuilder ?? throw new ArgumentNullException(nameof(ISessionBuilder));
        }

        private readonly AppDbContext _appDbContext;
        private readonly ISessionBuilder _sessionBuilder;
        public async Task<Session> BeginOrContinue(string logId, int testId)
        {
            Student student = await _appDbContext.Students.SingleOrDefaultAsync(s=>s.LogId == logId);
            if (student == null) throw new NotExistException("The student for specified LogId do not exists");
            Session session = await _appDbContext.Sessions.SingleOrDefaultAsync(s => s.Test.Id == testId && s.Student.Id == student.Id);
            if (session != null && (session.Finished)){
                _appDbContext.Sessions.Remove(session);
                await _appDbContext.SaveChangesAsync();
                session = null;
            } 
            if(session == null)
            {
                Test test = await _appDbContext.Tests
                                               .Where(t => t.Id == testId)
                                               .Include(t=>t.Questions)
                                               .FirstOrDefaultAsync();
                AccessToTest accessToTest = (await _appDbContext.AccessesToTest.FirstOrDefaultAsync(at => at.Test.Id == testId && at.Student.Id == student.Id));
                if (accessToTest == null) throw new DataAccessException($"The student {student.Id} hasn't got access to test {test.Id}");
                session = _sessionBuilder.Build(test, student);
                session.StartTime = DateTime.Now;
                _appDbContext.Sessions.Add(session);
                await _appDbContext.SaveChangesAsync();
            }
            return session;
        }

        public async Task<AnswerResult> Answer(string logId, int sessionId, object data)
        {
            Session session = await _appDbContext.Sessions
                                                 .Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                 .Include(s => s.Questions.Select(q=>q.Question))
                                                 .Include(s => s.Questions.Select(q => q.Question.Answer))
                                                 .Include(s => s.Questions.Select(q => q.Question.Answer.Addition))
                                                 .FirstOrDefaultAsync();
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
            _appDbContext.Sessions.Update(session);
            await _appDbContext.SaveChangesAsync();
            return answerResult;
        }

        public async Task<Session> GetSessionFor(int sessionId, string logId)
        {
            Session session = await _appDbContext.Sessions
                                                 .Where(s => s.Id == sessionId && s.Student.LogId == logId)
                                                 .Include(s=>s.Test)
                                                 .FirstOrDefaultAsync();
            if (session == null) throw new DataAccessException($"The student for specified LogId havn't got access to session({sessionId}) or the session doesn't exist");
            return session;
        }
    }
}
