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
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(512);
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Table)
                .WithMany(t => t.Seats)
                .HasForeignKey(s => s.TableId);
        }
    }
}
