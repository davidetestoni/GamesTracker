using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserGame>()
                .HasKey(k => new { k.SourceUserId, k.GameId });

            builder.Entity<UserGame>()
                .HasOne(s => s.SourceUser)
                .WithMany(g => g.Games)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserGame>()
                .HasOne(s => s.Game)
                .WithMany(g => g.PlayedBy)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
