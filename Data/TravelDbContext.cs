using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options)
        { }
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Place> Places => Set<Place>();
        public DbSet<HiddenGem> HiddenGems => Set<HiddenGem>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
            .HasMany(c => c.Cities)
            .WithOne(c => c.Country)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}