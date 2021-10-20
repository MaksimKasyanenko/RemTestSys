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
        public DbSet<OneOfFourVariantsAnswer> OneVariantAnswers { get; set; }
        public DbSet<SomeVariantsAnswer> SomeVariantsAnswers { get; set; }
        public DbSet<SequenceAnswer> SequenceAnswers { get; set; }
        public DbSet<ConnectedPairsAnswer> ConnectedPairsAnswers { get; set; }
        public DbSet<QuestionInSession> QuestionsInSessions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ResultOfTesting> ResultsOfTesting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        private void Fill()
        {
            Group group1 = new Group { Name = "Demo Group 1" };
            Groups.Add(group1);

            Test test1 = new Test { Name = "Demo Test", Description = "This is demo test", Duration = 1000, QuestionsCount = 8, ScoresPerRightAnswer=1.5 };
            Tests.Add(test1);
            Question[] questions = new Question[] {
                new Question{Test = test1, Text="Demo Question 1", Answer=new TextAnswer{RightText="demo1"} },
                new Question{Test = test1, Text="Demo Question 2", Answer=new OneOfFourVariantsAnswer{RightText="notfake",SerializedFakes="[\"fake1\",\"fake2\",\"fake3\"]"} },
                new Question{Test=test1,Text="Demo Question 3",Answer = new SomeVariantsAnswer{SerializedRightAnswers="[\"right1\",\"right2\"]", SerializedFakes="[\"fake1\",\"fake2\"]"} },
                new Question{Test = test1, Text="Demo Question 4", Answer=new SequenceAnswer{SerializedSequence="[\"1\",\"2\",\"4\"]"} },
                new Question{Test = test1, Text="Demo Question 5", Answer=new ConnectedPairsAnswer{SerializedPairs="[{\"Value1\":\"a\",\"Value2\":\"A\"},{\"Value1\":\"b\",\"Value2\":\"B\"}]"} }
            };
            TextAnswers.Add((TextAnswer)questions[0].Answer);
            Questions.Add(questions[0]);
            OneVariantAnswers.Add((OneOfFourVariantsAnswer)questions[1].Answer);
            Questions.Add(questions[1]);
            SomeVariantsAnswers.Add((SomeVariantsAnswer)questions[2].Answer);
            Questions.Add(questions[2]);
            SequenceAnswers.Add((SequenceAnswer)questions[3].Answer);
            Questions.Add(questions[3]);
            ConnectedPairsAnswers.Add((ConnectedPairsAnswer)questions[4].Answer);
            Questions.Add(questions[4]);

            AccessesToTest.Add(new AccessToTest {Group=group1, Test=test1});
            SaveChanges();
        }
    }
}
