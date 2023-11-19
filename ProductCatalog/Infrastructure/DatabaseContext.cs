using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.ApplyProductReviews;
using ProductCatalog.Contract;

namespace ProductCatalog.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        
        public DbSet<Review> Reviews { get; set; } = null!;

        public DbSet<Checkpoint> Checkpoints { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseInMemoryDatabase("Database");
    }
}
