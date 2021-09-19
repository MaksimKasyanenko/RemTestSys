using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Models;
using System.Collections.Generic;

namespace RemTestSys.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Student> Students { get; set; }
        public DbSet<AccessToTest> AccessesToTest { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}
