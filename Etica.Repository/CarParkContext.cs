using Etica.Repository.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Etica.Repository
{
    public class CarParkContext : DbContext
    {
        public DbSet<RateEntity> Rates { get; set; }

        public DbSet<HourlyRateEntity> HourlyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=CarPark.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateEntity>().ToTable("Rates");
            modelBuilder.Entity<HourlyRateEntity>().ToTable("HourlyRates");
            modelBuilder.Entity<RateEntity>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
