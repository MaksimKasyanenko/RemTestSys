using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;

namespace RemTestSys
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
            if (Database.EnsureCreated())
            {
                Fill();
            }
        }

        public DbSet<Student> Students { get; internal set; }
        public DbSet<AccessToTest> AccessesToTest { get; internal set; }
        public DbSet<Test> Tests { get; internal set; }
        public DbSet<Session> Sessions { get; internal set; }
        public DbSet<Question> Questions { get; internal set; }
        public DbSet<TextAnswer> TextAnswers { get; internal set; }
        public DbSet<QuestionInSession> QuestionsInSessions { get; internal set; }
        public DbSet<Group> Groups { get; internal set; }
        public DbSet<ResultOfTesting> ResultsOfTesting { get; internal set; }

        private void Fill()
        {
            Group group1 = new Group { Name = "Demo Group 1" };
            Group group2 = new Group { Name = "Demo Group 2" };
            Groups.Add(group1);
            Groups.Add(group2);

            Test test1 = new Test { Name = "Demo Test", Description = "This is demo test", Duration = 1000, QuestionsCount = 2, ScoresPerRightAnswer=6 };
            Tests.Add(test1);
            Question[] questions = new Question[] {
                new Question{Test = test1, Text="Demo Question 1", Answer=new TextAnswer{RightText="demo1"} },
                new Question{Test = test1, Text="Demo Question 2", Answer=new TextAnswer{RightText="demo2"} }
            };
            foreach(var q in questions)
            {
                TextAnswers.Add((TextAnswer)q.Answer);
                Questions.Add(q);
            }

            Test test2 = new Test { Name = "Demo Test 2", Description = "This is demo test 2", Duration = 100, QuestionsCount = 4, ScoresPerRightAnswer = 3 };
            Tests.Add(test2);
            questions = new Question[] {
                new Question{Test = test2, Text="Demo Question 3", Answer=new TextAnswer{RightText="demo3"} },
                new Question{Test = test2, Text="Demo Question 4", Answer=new TextAnswer{RightText="demo4"} }
            };
            foreach (var q in questions)
            {
                TextAnswers.Add((TextAnswer)q.Answer);
                Questions.Add(q);
            }

            AccessesToTest.Add(new AccessToTest {Group=group1, Test=test1});
            AccessesToTest.Add(new AccessToTest { Group = group2, Test = test2 });
            SaveChanges();
        }
    }
}
