using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;

namespace RemTestSys.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Student> Students { get; set; }
        public DbSet<AccessToTestForStudent> AccessesToTestForStudent { get; set; }
        public DbSet<AccessToTestForAll> AccessesToTestForAll { get; set; }
        public DbSet<AccessToTestForGroup> AccessesToTestForGroup { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Test.MapPart> MapParts{get;set;}
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
        public DbSet<TestViewModel> TestViewModel { get; set; }
    }
}
