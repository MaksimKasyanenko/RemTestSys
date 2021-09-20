using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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
        public DbSet<Exam> Exams { get; internal set; }
        public DbSet<Session> Sessions { get; internal set; }
        public DbSet<Question> Questions { get; internal set; }
        public DbSet<Answer> Answers { get; internal set; }
        public DbSet<QuestionInSession> QuestionsInSessions { get; internal set; }
        public DbSet<Group> Groups { get; internal set; }

        private void Fill()
        {
            Group group = new Group { Name = "Demo Group" };
            Student student = new Student {FirstName="Demo", LastName="Student", Group=group, LogId="demo"};

        }
    }
}
