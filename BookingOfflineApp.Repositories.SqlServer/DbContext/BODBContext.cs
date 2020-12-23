using BookingOfflineApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingOfflineApp.Repositories.SqlServer
{
    public class BODBContext : DbContext
    {
        public DbSet<AlipayUser> AlipayUsers { get; set; }
        public DbSet<WechatUser> WechatUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemOption> OrderItemOptions { get; set; }

        public BODBContext(DbContextOptions<BODBContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(s => s.OrderItems)
                .WithOne(s => s.Order)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired(true);
            modelBuilder.Entity<OrderItem>()
                .HasMany(s => s.OrderItemOptions)
                .WithOne(s => s.OrderItem)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired();
        }
    }
}
