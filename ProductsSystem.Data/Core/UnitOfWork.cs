using ProductsSystem.Data.Repositories;
using ProductsSystem.Models.EntityModels;
using System.Threading.Tasks;

namespace ProductsSystem.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProductsSystemDbContext _context;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SubCategory> _subCategoryRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<User> _userRepository;

        public UnitOfWork(ProductsSystemDbContext context)
        {
            this._context = context;
        }

        public IGenericRepository<Category> CategoryRepository => _categoryRepository ?? new GenericRepository<Category>(_context);

        public IGenericRepository<SubCategory> SubCategoryRepository => _subCategoryRepository ?? new GenericRepository<SubCategory>(_context);

        public IGenericRepository<Product> ProductRepository => _productRepository ?? new GenericRepository<Product>(_context);

        public IGenericRepository<User> UserRepository => _userRepository ?? new GenericRepository<User>(_context);

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
