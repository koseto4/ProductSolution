using ProductsSystem.Data.Repositories;
using ProductsSystem.Models.EntityModels;
using System.Threading.Tasks;

namespace ProductsSystem.Data.Core
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<SubCategory> SubCategoryRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
