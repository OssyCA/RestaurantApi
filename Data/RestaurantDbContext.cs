using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using System;

namespace RestaurantApi.Data
{
    public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Phone)
                .IsUnique();
            
            modelBuilder.Entity<RestaurantTable>()
               .HasIndex(t => t.TableNumber)
               .IsUnique();

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.ExpireAt)
                      .HasComputedColumnSql("DATEADD(hour, 2, [StartAt])");
            });

        }
    }
}
