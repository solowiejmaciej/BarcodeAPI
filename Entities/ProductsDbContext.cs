using Microsoft.EntityFrameworkCore;
using BarcodeAPI.Entities;

namespace BarcodeAPI.Entities
{
    public class ProductsDbContext : DbContext
    {
        private string _connectionString = "Server=localhost,1433;Database=Products;User Id=sa;Password=Database!2023;Trusted_Connection=False;TrustServerCertificate=True;";

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Product>()
                .Property(p => p.Ean)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}