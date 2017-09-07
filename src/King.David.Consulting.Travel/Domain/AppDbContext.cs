using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace King.David.Consulting.Travel.Web.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<UserVisit> UserVisits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Map(modelBuilder.Entity<UserVisit>());
        }

        private void Map(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => new { k.UserId });
        }

        private void Map(EntityTypeBuilder<UserVisit> builder)
        {

            builder.HasKey(t=> new {t.UserId, t.CityId, t.StateId});
     
            builder.HasOne(pt => pt.User)
                .WithMany(p => p.UserVisits)
                .HasForeignKey(pt => pt.UserId);

            builder.HasOne(pt => pt.City)
                .WithMany(p => p.UserVisits)
                .HasForeignKey(pt => pt.CityId);

            builder.HasOne(pt => pt.State)
                    .WithMany(p => p.UserVisits)
                    .HasForeignKey(pt => pt.StateId);
        }
    }
}
