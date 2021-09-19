using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using System.Collections.Generic;

namespace RemTestSys.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Student> Students { get; internal set; }
        public DbSet<AccessToTest> AccessesToTest { get; internal set; }
        public DbSet<Test> Tests { get; internal set; }
        public DbSet<Exam> Exams { get; internal set; }
        public DbSet<Session> Sessions { get; internal set; }
    }
}
