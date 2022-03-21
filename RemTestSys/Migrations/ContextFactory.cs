using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RemTestSys.Domain;

namespace RemTestSys.Migrations
{
    public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string connectionString = @"Server=(localdb)\himsqlins;Integrated Security=true;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}