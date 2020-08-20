using Etica.Repository.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Etica.Repository
{
    public class CarParkContext : DbContext
    {
        public DbSet<RateEntity> Rates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=CarPark.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateEntity>().ToTable("Rates");
            modelBuilder.Entity<RateEntity>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
