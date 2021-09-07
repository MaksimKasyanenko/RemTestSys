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
        public SessionBuilder(ITestsDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(ITestsDbContext));
        }

        private readonly ITestsDbContext _dbContext;

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

        private class RandomSequence
        {
            int[] _seq;
            int _cursor;
            static Random rnd = new Random();
            public RandomSequence(int start, int count)
            {
                if (count < 1) throw new ArgumentException("count");
                _seq = new int[count];
                for(int i = 0; i < _seq.Length; i++)
                {
                    _seq[i] = start++;
                }
                Mix();
                _cursor = -1;
            }

            public int GetNext()
            {
                _cursor++;
                if (_cursor >= _seq.Length) _cursor = 0;
                return _seq[_cursor];
            }

            private void Mix()
            {
                for(int i=0; i < _seq.Length; i++)
                {
                    int boof = _seq[i];
                    int r = rnd.Next(i, _seq.Length);
                    _seq[i] = _seq[r];
                    _seq[r] = boof;
                }
            }
        }
    }
}
