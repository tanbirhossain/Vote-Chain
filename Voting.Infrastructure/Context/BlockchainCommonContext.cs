using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Voting.Model.Entities;

namespace Voting.Model.Context
{
    public class BlockchainCommonContext : DbContext
    {
        public BlockchainCommonContext(DbContextOptions<BlockchainCommonContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<ElectionCandidate> ElectionCandidates { get; set; }
    }

    public class DesignTimeCommonDbContextFactory : IDesignTimeDbContextFactory<BlockchainCommonContext>
    {
        public BlockchainCommonContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Voting.WEB"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BlockchainCommonContext>();
            var connectionString = config.GetConnectionString("BlockchainCommonContext");

            //optionsBuilder.UseSqlServer(connectionString);

            optionsBuilder.UseSqlite("Filename=./wwwroot/data/CommonDatabase.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            var context = new BlockchainCommonContext(optionsBuilder.Options);

            return context;
        }
    }
}