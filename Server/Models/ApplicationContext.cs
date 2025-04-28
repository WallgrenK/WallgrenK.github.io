using Microsoft.EntityFrameworkCore;
using Server.Models.SeatingModels;
using Server.Models.UserModels;

namespace Server.Models
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(u => u.Username)
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
        }
    }
}
