using Microsoft.EntityFrameworkCore;
using src.Entity;

namespace src.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }

        public DatabaseContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.UserRole).HasConversion<string>();

            modelBuilder
                .Entity<User>()
                .HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey(order => order.UserID)
                .HasPrincipalKey(user => user.UserID);

            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();
        }
    }
}
