using Microsoft.EntityFrameworkCore;
using API.Data.Entities;

namespace API.Data
{
    public class GameDbContext : DbContext
    {
        public DbSet<Save> Saves { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Game;Username=postgres;Password=postgres");
        }
    }
}
