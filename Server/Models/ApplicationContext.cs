using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models.SeatingModels;
using Server.Models.SecurityModels;
using Server.Models.UserModels;

namespace Server.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TableName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(t => t.Seats)
                    .WithOne(s => s.Table)
                    .HasForeignKey(s => s.TableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithOne(u => u.BookedSeat)
                    .HasForeignKey<Seat>(s => s.BookedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(s => s.BookedByUserId)
                    .IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Token).IsRequired()
                .HasMaxLength(150);

                entity.Property(t => t.UserId).IsRequired()
                .HasMaxLength(512);

                entity.HasOne(t => t.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
