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

        public DbSet<Student> Students { get; set; }
        public DbSet<AccessToTest> AccessesToTest { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TextAnswer> TextAnswers { get; set; }
        public DbSet<OneVariantAnswer> OneVariantAnswers { get; set; }
        public DbSet<QuestionInSession> QuestionsInSessions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ResultOfTesting> ResultsOfTesting { get; set; }

        private void Fill()
        {
            Group group1 = new Group { Name = "Demo Group 1" };
            Groups.Add(group1);

            Test test1 = new Test { Name = "Demo Test", Description = "This is demo test", Duration = 1000, QuestionsCount = 2, ScoresPerRightAnswer=6 };
            Tests.Add(test1);
            Question[] questions = new Question[] {
                new Question{Test = test1, Text="Demo Question 1", Answer=new TextAnswer{RightText="demo1"} },
                new Question{Test = test1, Text="Demo Question 2", Answer=new OneVariantAnswer{RightText="notfake",SerializedFakes="[\"fake1\",\"fake2\",\"fake3\"]"} }
            };
            TextAnswers.Add((TextAnswer)questions[0].Answer);
            Questions.Add(questions[0]);
            OneVariantAnswers.Add((OneVariantAnswer)questions[1].Answer);
            Questions.Add(questions[1]);

            AccessesToTest.Add(new AccessToTest {Group=group1, Test=test1});
            SaveChanges();
        }
    }
}
