using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Zeiterfassung.models;

namespace Zeiterfassung.services
{
    public class ZeiterfassungsDBContext : DbContext
    {
        private static readonly string _databaseName = "Datenbank";
        private static string _connection = ConfigurationManager.ConnectionStrings[_databaseName].ConnectionString;

        private static ZeiterfassungsDBContext _instance;
        public static ZeiterfassungsDBContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ZeiterfassungsDBContext();
                return _instance;
            }
        }

        public DbSet<Person> Personen { get; set; }
        public DbSet<PersonenSollzeitModelle> PersonenSollzeitModelle { get; set; }
        public DbSet<SollzeitModelle> SollzeitModelle { get; set; }
        public DbSet<SollzeitModelleZeiten> SollzeitModelleZeiten { get; set; }
        public DbSet<Arbeitszeit> Arbeitszeit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonenSollzeitModelle>().HasNoKey();
            modelBuilder.Entity<SollzeitModelleZeiten>().HasNoKey();
        }
    }
}
