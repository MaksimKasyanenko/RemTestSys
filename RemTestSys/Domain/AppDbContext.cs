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
        public DbSet<ResultOfTesting> ResultsOfTesting { get; internal set; }

        private void Fill()
        {
            Group group = new Group { Name = "Demo Group" };
            Student student = new Student {FirstName="Demo", LastName="Student", Group=group, LogId="demo"};
            Groups.Add(group);
            Students.Add(student);
            Test test = new Test { Name = "Demo Test", Description = "This is demo test", Duration = 1000, QuestionsCount = 2 };
            Tests.Add(test);
            Question[] questions = new Question[] {
                new Question{Test = test, Text="Demo Question 1", Answer=new Answer{RightText="demo1"} },
                new Question{Test = test, Text="Demo Question 2", Answer=new Answer{RightText="demo2"} }
            };
            foreach(var q in questions)
            {
                Answers.Add(q.Answer);
                Questions.Add(q);
            }
            AccessesToTest.Add(new AccessToTest {Student=student, Test=test});
            Exam exam = new Exam {
                AssignedTo = student,
                Status=Domain.ExamStatus.NotPassed,
                IsStrict=false,
                Deadline=new System.DateTime(2024,1,1,0,0,0),
                Test=test
            };
            Exams.Add(exam);
            SaveChanges();
        }
    }
}
