using Microsoft.EntityFrameworkCore;
using ProductsSystem.Models.EntityModels;

namespace ProductsSystem.Data.Core
{
    public class ProductsSystemDbContext : DbContext
    {
        public ProductsSystemDbContext(DbContextOptions<ProductsSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
