using Microsoft.EntityFrameworkCore;
using System;

namespace AutoUI.Queue
{
    public class QueueDbContext : DbContext
    {
        public QueueDbContext()
        {
        }
        public QueueDbContext(DbContextOptions<QueueDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=queue_db;Username=postgres;Password=12345");
        }
        public DbSet<Run> Runs { get; set; }


    }
}
