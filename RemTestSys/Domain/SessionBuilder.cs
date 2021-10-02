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
            Question[] questionsOfTest = test.Questions.ToArray();
            questionsOfTest = GetRandomSet(questionsOfTest, test.QuestionsCount);
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

        private Question[] GetRandomSet(Question[] src, int size)
        {
            Question[] resultSet = new Question[size];
            RandomSequence sequence = new RandomSequence(0, src.Length);
            for(int i = 0; i < resultSet.Length; i++)
            {
                resultSet[i] = src[sequence.GetNext()] ;
            }
            return resultSet;
        }
    }
}
