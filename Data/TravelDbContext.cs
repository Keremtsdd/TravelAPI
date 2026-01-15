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
        public DbSet<Favorites> Favorites => Set<Favorites>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.Country)
                .WithMany()
                .HasForeignKey(f => f.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.City)
                .WithMany()
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.Place)
                .WithMany()
                .HasForeignKey(f => f.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}