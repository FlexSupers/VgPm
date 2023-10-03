using Microsoft.EntityFrameworkCore;
using VG.Pm.PmDb.Models;
using VG.Pm.PmDb.Shared;

namespace VG.Pm.PmDb
{
    public class PmDbContext : DbContext
    {
        public PmDbContext(DbContextOptions<PmDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<LogApplicationError> DbSetLogApplication { get; set; }
        public DbSet<Project> DbSetProject { get; set; }
        public DbSet<Models.Task> DbSetTask { get; set; }
        public DbSet<Status> DbSetStatus { get; set; }
        public DbSet<TaskType> DbSetTaskType { get; set; }
    }
}