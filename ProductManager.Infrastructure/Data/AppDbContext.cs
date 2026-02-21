using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Description)
                    .HasMaxLength(1000);

                entity.Property(p => p.Category)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(p => p.Status)
                    .IsRequired();
            });
        }
    }
}
