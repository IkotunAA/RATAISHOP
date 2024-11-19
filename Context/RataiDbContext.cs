using Microsoft.EntityFrameworkCore;
using RATAISHOP.Entities;
using System;
namespace RATAISHOP.Context
{
   

        public class RataiDbContext : DbContext
        {
            public DbSet<User> Users { get; set; } = default!;
            public DbSet<Product> Products { get; set; } = default!;
            public DbSet<Order> Orders { get; set; } = default!;
            public DbSet<OrderItem> OrderItems { get; set; } = default!;
            public DbSet<CartItem> CartItems { get; set; } = default!;
            public DbSet<Wallet> Wallets { get; set; } = default!;

        public RataiDbContext(DbContextOptions<RataiDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>()
                    .HasMany(u => u.Orders)
                    .WithOne(o => o.Buyer)
                    .HasForeignKey(o => o.BuyerId);

                modelBuilder.Entity<User>()
                    .HasMany(u => u.Products)
                    .WithOne(p => p.Seller)
                    .HasForeignKey(p => p.SellerId);

                modelBuilder.Entity<User>()
                    .HasMany(u => u.CartItems)
                    .WithOne(ci => ci.Buyer)
                    .HasForeignKey(ci => ci.BuyerId);

                modelBuilder.Entity<Product>()
                    .HasMany(p => p.CartItems)
                    .WithOne(ci => ci.Product)
                    .HasForeignKey(ci => ci.ProductId);

               
            }
        }


}
