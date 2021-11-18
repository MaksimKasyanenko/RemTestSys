using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public class SessionBuilder : ISessionBuilder
    {
        public SessionBuilder(AppDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private readonly AppDbContext _dbContext;

        public Session Build(Test test, Student student)
        {
            Session session = new Session();
            session.Test = test;
            session.Student = student;
            session.QuestionNum = 1;
            session.Finished = false;
            Question[] questionsOfTest = GetRandomSet(test);
            for(int i=1; i<=questionsOfTest.Length;i++)
            {
                session.Questions.Add(new QuestionInSession {
                    Session = session,
                    Question = questionsOfTest[i-1],
                    SerialNumber=i
                });
            }
            return session;
        }

        private Question[] GetRandomSet(Test test)
        {
            Question[] resultSet = new Question[test.QuestionsCount];
            int cursor=0;
            int oldCursor = 0;
            foreach(var part in test.MapParts.OrderBy(mp=>mp.QuestionCast))
            {
                Question[] questions = test.Questions.Where(q=>q.Cast==part.QuestionCast).ToArray();
                RandomSequence sequence = new RandomSequence(0, questions.Length);
                for(; cursor < oldCursor+part.QuestionCount; cursor++)
                {
                    resultSet[cursor] = questions[sequence.GetNext()];
                }
                oldCursor = cursor;
            }
            return resultSet;
        }
    }
}
