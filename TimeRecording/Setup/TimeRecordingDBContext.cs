using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TimeRecording.Models;

namespace TimeRecording.Setup
{
    public class TimeRecordingDBContext : DbContext
    {
        private static readonly string _databaseName = "Database";
        private static string _connection = ConfigurationManager.ConnectionStrings[_databaseName].ConnectionString;

        public DbSet<Person> Person { get; set; }
        public DbSet<PersonTargetTimeModel> PersonTargetTimeModel { get; set; }
        public DbSet<TargetTimeModel> TargetTimeModel { get; set; }
        public DbSet<TargetTimeModelTimes> TargetTimeModelTimes { get; set; }
        public DbSet<WorkingTime> WorkingTime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PersonTargetTimeModel>().HasNoKey();
            modelBuilder.Entity<TargetTimeModelTimes>().HasNoKey();
            modelBuilder.Entity<WorkingTime>().HasKey(wt => new { wt.PersonId, wt.Date });
        }
    }
}
