using Microsoft.EntityFrameworkCore;
using API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Data
{
    public class GameDbContext : IdentityDbContext<GameUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Save> Saves { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Worker> Workers { get; set; }

        public GameDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetValue<string>("PostgreSQLConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Building>()
                .HasOne(building => building.Save)
                .WithMany()
                .HasForeignKey(Building => Building.SaveId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Worker>()
                .HasOne(worker => worker.Building)
                .WithMany()
                .HasForeignKey(worker => worker.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
